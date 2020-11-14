using CSharpUrl.LibCurl;
using CSharpUrl.LibUv;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.Threading;

namespace CSharpUrl
{
    public enum RetryOrReset
    {
        Retry,
        Reset
    }

    public class ReqCtx<TReqState>
    {
        public TReqState ReqState { get; }
        public Action<CurlEzHandle, TReqState> ConfigureEzReq { get; }
        public Func<CurlEzHandle, TReqState, CURLcode, RetryOrReset> HandleResp { get; }
        public Action<TReqState> Cancel { get; }
        public CancellationToken CancellationToken { get; }
        
        public ReqCtx(
            TReqState reqState,
            Action<CurlEzHandle, TReqState> configureEzReq,
            Func<CurlEzHandle, TReqState, CURLcode, RetryOrReset> handleResp,
            Action<TReqState> cancel,
            CancellationToken cancellationToken)
        {
            ReqState = reqState;
            ConfigureEzReq = configureEzReq;
            HandleResp = handleResp;
            Cancel = cancel;
            CancellationToken = cancellationToken;
        }
    }

    internal class CurlMultiSocketAgentState<TReqState>
    {
        public CurlMultiHandle Multi { get; }
        public libcurl.socket_callback SocketCallback { get; }
        public libcurl.timer_callback TimerCallback { get; }
        public Dictionary<IntPtr, ReqCtx<TReqState>> ActiveRequests { get; }
        public Queue<CurlEzHandle> InactiveEzHandles { get; }
        public Dictionary<int, UvPoll> Sockets { get; }
        public Queue<ReqCtx<TReqState>> PendingRequests { get; }
        public Queue<CurlEzHandle> EzsToAdd { get; }
        public GCHandle Handle { get; }
        public ReadOnlyDictionary<IntPtr, CurlEzHandle> Easys { get; }
        public UvLoop Loop { get; }
        public UvTimer Timer { get; }
        public UvAsync AddMulti { get; }
        public UvAsync Disposer { get; }
        public UvAsync Cancel { get; }
        public Dictionary<IntPtr, IDisposable> CancellationRegistrations { get; }
        public Queue<CurlEzHandle> EzsToCancel { get; }
        public bool Initialized { get; set; }

        public CurlMultiSocketAgentState(
            CurlMultiHandle multi,
            libcurl.timer_callback timerCallback,
            libcurl.socket_callback socketCallback,
            ReadOnlyDictionary<IntPtr, CurlEzHandle> easys,
            UvLoop loop,
            UvTimer timer,
            UvAsync addMulti,
            UvAsync disposer,
            UvAsync cancel)
        {
            (Multi, TimerCallback, SocketCallback, Easys) = (multi, timerCallback, socketCallback, easys);
            (Loop, Timer, AddMulti, Disposer) = (loop, timer, addMulti, disposer);
            InactiveEzHandles = new Queue<CurlEzHandle>(easys.Count);
            foreach (var kvp in easys)
                InactiveEzHandles.Enqueue(kvp.Value);
            ActiveRequests = new Dictionary<IntPtr, ReqCtx<TReqState>>(easys.Count);
            Sockets = new Dictionary<int, UvPoll>(easys.Count);
            PendingRequests = new Queue<ReqCtx<TReqState>>(easys.Count);
            EzsToAdd = new Queue<CurlEzHandle>(easys.Count);
            CancellationRegistrations = new Dictionary<IntPtr, IDisposable>();
            EzsToCancel = new Queue<CurlEzHandle>();
            Cancel = cancel;
            Handle = GCHandle.Alloc(this);
        }
    }

    internal static class Agent
    {
        private static CurlMultiSocketAgentState<TReqState> State<TReqState>(UvHandle handle)
        {
            if (handle.Data == null)
                throw new InvalidOperationException("uv_handle data field is null.");
            return (CurlMultiSocketAgentState<TReqState>)handle.Data;
        }

        private static CurlMultiSocketAgentState<TReqState> State<TReqState>(IntPtr user)
        {
            var gcHandle = GCHandle.FromIntPtr(user);
            if (!gcHandle.IsAllocated)
                throw new InvalidOperationException("expected gchandle to be alloc'd.");
            var state = (CurlMultiSocketAgentState<TReqState>)gcHandle.Target;
            return state;
        }

        private static void CurlMultiAddEzs<TReqState>(UvAsync asy)
        {
            var state = State<TReqState>(asy);
            lock (state) {
                while (state.EzsToAdd.Count > 0) {
                    var ez = state.EzsToAdd.Dequeue();
                    var reqCtx = state.ActiveRequests[ez];
                    reqCtx.ConfigureEzReq(ez, reqCtx.ReqState);
                    Curl.MultiAddEz(state.Multi, ez);
                }
            }

            if (state.Initialized)
                return;

            state.Initialized = true;
            Curl.CurlMultiSocketAction(state.Multi, sockfd: -1, CURLcselect.NONE);
        }

        private static void Cancel<TReqState>(UvAsync asy)
        {
            var state = State<TReqState>(asy);
            lock (state) {
                while (state.EzsToCancel.Count > 0) {
                    var ez = state.EzsToCancel.Dequeue();
                    
                    if (state.EzsToAdd.Contains(ez)) {
                        while (true) {
                            var value = state.EzsToAdd.Dequeue();
                            if (value != ez) {
                                state.EzsToAdd.Enqueue(value);
                                continue;
                            }

                            break;
                        }
                    }
                    
                    if (!state.ActiveRequests.ContainsKey(ez)) {
#if DEBUG
                        Console.Error.WriteLine($"WARNING: expected active requests to contain key {(IntPtr)ez}");
                        var _tmp = state.CancellationRegistrations.ContainsKey(ez);
#endif
                    }
                    else {
                        var reqCtx = state.ActiveRequests[ez];
                        ResetEz(state, ez);
                        reqCtx.Cancel(reqCtx.ReqState);
                    }
                }
            }
        }
        
        private static (
            UvLoop,
            UvTimer,
            UvAsync,
            UvAsync,
            UvAsync
        ) InitUvHandles<TReqState>()
        {
            var loop = new UvLoop();
            var timer = new UvTimer(loop);
            var addMulti = new UvAsync(loop, CurlMultiAddEzs<TReqState>);
            var disposer = new UvAsync(loop, FreeMemory<TReqState>);
            var cancel = new UvAsync(loop, Cancel<TReqState>);
            return (loop, timer, addMulti, disposer, cancel);
        }

        private static (CurlMultiHandle multi, ReadOnlyDictionary<IntPtr, CurlEzHandle> ezs) InitCurl(
            int ezHandleCount)
        {
            var multi = libcurl.curl_multi_init();
            //Curl.MultiSetOpt(multi, CURLMoption.MAXCONNECTS, 3);
            Curl.MultiSetOpt(multi, CURLMoption.MAX_TOTAL_CONNECTIONS, 55);
            Curl.MultiSetOpt(multi, CURLMoption.MAX_HOST_CONNECTIONS, 5);

            var handsUp = new Dictionary<IntPtr, CurlEzHandle>(ezHandleCount);
            for (var i = 0; i < ezHandleCount; i++) {
                var handle = libcurl.curl_easy_init();
                handsUp.Add(handle, handle);
            }
            var ezClap = new ReadOnlyDictionary<IntPtr, CurlEzHandle>(handsUp);
            return (multi, ezClap);
        }

        public static CurlMultiSocketAgentState<TReqState> Activate<TReqState>(
            int ezHandleCount)
        {
            CurlMultiSocketAgentState<TReqState>? state = default;
            using var waitHandle = new ManualResetEventSlim();
            var t = new Thread(
                () => {
                    var (multi, ezClap) = InitCurl(ezHandleCount);
                    var (loop, timer, addMulti, disposer, cancel) = InitUvHandles<TReqState>();
                    using var _ = loop;
                    var s = new CurlMultiSocketAgentState<TReqState>(
                        multi: multi,
                        timerCallback: HandleTimer<TReqState>,
                        socketCallback: ProcessSocket<TReqState>,
                        easys: ezClap,
                        loop: loop,
                        timer: timer,
                        addMulti: addMulti,
                        disposer: disposer,
                        cancel: cancel
                    );
                    state = s;
                    loop.Data = state;
                    timer.Data = state;
                    addMulti.Data = state;
                    disposer.Data = state;
                    cancel.Data = state;
                    var statePtr = GCHandle.ToIntPtr(s.Handle);
                    Curl.MultiSetOpt(state.Multi, CURLMoption.SOCKETFUNCTION, state.SocketCallback);
                    Curl.MultiSetOpt(state.Multi, CURLMoption.SOCKETDATA, statePtr);
                    Curl.MultiSetOpt(state.Multi, CURLMoption.TIMERFUNCTION, state.TimerCallback);
                    Curl.MultiSetOpt(state.Multi, CURLMoption.TIMERDATA, statePtr);
                    // ReSharper disable once AccessToDisposedClosure
                    waitHandle.Set();
                    Exe(loop);
                }
            ) { IsBackground = true };
            t.Start();
            waitHandle.Wait();

            return state!;
        }

        public static void Exe(UvLoop loop)
        {
            UvModule.ValidateResult(libuv.uv_run(loop, uv_run_mode.UV_RUN_DEFAULT));
        }

        public static int ProcessSocket<TReqState>(
            IntPtr _ez,
            int sockfd,
            CURLpoll what,
            IntPtr userData,
            IntPtr _socketData)
        {
            var state = State<TReqState>(userData);
            switch (what) {
                case CURLpoll.IN:
                case CURLpoll.OUT:
                case CURLpoll.INOUT: return BeginPoll(state, what, sockfd);
                case CURLpoll.REMOVE: return DisposePoll(state, sockfd);
                default: throw new InvalidOperationException($"Invalid CURLpoll received: {what}.");
            }
        }

        private static void SignalTimeout<TReqState>(UvTimer timer)
        {
            var state = State<TReqState>(timer);
            var _active = Curl.CurlMultiSocketAction(state.Multi, sockfd: -1, CURLcselect.NONE);
            ProcessMultiData(state);
        }

        public static int HandleTimer<TReqState>(IntPtr multi, int timeoutMs, IntPtr userp)
        {
            var state = State<TReqState>(userp);
            if (timeoutMs < 0) {
                state.Timer.Stop();
            } else {
                state.Timer.Start(SignalTimeout<TReqState>, timeoutMs, 0);
            }
            return 0;
        }

        public static void EnqueueRequest<TReqState>(
            CurlMultiSocketAgentState<TReqState> state,
            ReqCtx<TReqState> reqCtx)
        {
            lock (state) {
                if (state.InactiveEzHandles.Count > 0) {
                    var ez = state.InactiveEzHandles.Dequeue();
                    state.ActiveRequests[ez] = reqCtx;
                    var reg = reqCtx.CancellationToken.Register(() => {
                        lock (state) {
                            state.EzsToCancel.Enqueue(ez);
                            state.Cancel.Send();
                        }
                    });
                    state.CancellationRegistrations[ez] = reg;
                    state.EzsToAdd.Enqueue(ez);
                    state.AddMulti.Send();
                } else {
                    state.PendingRequests.Enqueue(reqCtx);
                }
            }
        }

        public static int DisposePoll<TReqState>(
            CurlMultiSocketAgentState<TReqState> state,
            int sockfd)
        {
            using var poll = state.Sockets[sockfd];
            poll.Stop();
            state.Sockets.Remove(sockfd);
            return 0;
        }

        public static void RetryEz<TReqState>(
            CurlMultiSocketAgentState<TReqState> state,
            CurlEzHandle easy)
        {
            Curl.MultiRemoveEz(state.Multi, easy);
            Curl.MultiAddEz(state.Multi, easy);
        }

        private static void ClearEzCookies(CurlEzHandle easy)
        {
            var result = libcurl.curl_easy_setopt(easy, CURLoption.COOKIELIST, "ALL");
            Curl.ValidateSetOptResult(result);
        }

        public static void ResetEz<TReqState>(
            CurlMultiSocketAgentState<TReqState> state,
            CurlEzHandle ez)
        {
            Curl.MultiRemoveEz(state.Multi, ez);
            ClearEzCookies(ez);
            libcurl.curl_easy_reset(ez);

            ReqCtx<TReqState>? next = default;

            lock (state) {
                var _res = state.ActiveRequests.Remove(ez);
                state.CancellationRegistrations[ez].Dispose();
                state.CancellationRegistrations.Remove(ez);
                state.InactiveEzHandles.Enqueue(ez);
                if (state.PendingRequests.Count > 0)
                    next = state.PendingRequests.Dequeue();
            }

            if (next != null)
                EnqueueRequest(state, next);
        }

        private static void ProcessPollResult(UvPoll poll, int status, int events)
        {
            var state = State<HttpReqState>(poll);

            if (status != 0) {
                Curl.CurlMultiSocketAction(state.Multi, poll.Fd, CURLcselect.ERR);
                ProcessMultiData(state);
                return;
            }

            var mask = (uv_poll_event)events;

            CURLcselect flags = CURLcselect.NONE;
            if ((mask & uv_poll_event.UV_READABLE) != 0)
                flags |= CURLcselect.IN;

            if ((mask & uv_poll_event.UV_WRITABLE) != 0)
                flags |= CURLcselect.OUT;

            var _active = Curl.CurlMultiSocketAction(state.Multi, poll.Fd, flags);
            ProcessMultiData(state);
        }

        public static int BeginPoll<TReqState>(
            CurlMultiSocketAgentState<TReqState> state,
            CURLpoll what,
            int sockfd)
        {
            var events = uv_poll_event.NONE;

            if (what != CURLpoll.IN)
                events |= uv_poll_event.UV_WRITABLE;

            if (what != CURLpoll.OUT)
                events |= uv_poll_event.UV_READABLE;

            if (!state.Sockets.TryGetValue(sockfd, out var poll)) {
                poll = new UvPoll(state.Loop, sockfd) { Data = state };
                state.Sockets.Add(sockfd, poll);
            }

            poll.Start(events, ProcessPollResult);
            return 0;
        }


        public static void ProcessMultiData<TReqState>(CurlMultiSocketAgentState<TReqState> state)
        {
            IntPtr pMessage;
            while ((pMessage = libcurl.curl_multi_info_read(state.Multi, out _)) != IntPtr.Zero) {
                var message = Marshal.PtrToStructure<CURLMsg>(pMessage);
                if (message.msg != CURLMSG.DONE)
                    throw new CurlException($"Unexpected curl_multi_info_read result message: {message.msg}.");

                var ez = state.Easys[message.easy_handle];
                var ezPtrClap = (IntPtr)ez;
                if (!state.ActiveRequests.ContainsKey(ezPtrClap)) {
                    throw new InvalidOperationException(
                        "curl_multi_info_read returned an inactive ez handle."
                    );
                }

                var reqCtx = state.ActiveRequests[ezPtrClap];

                switch (reqCtx.HandleResp(ez, reqCtx.ReqState, message.data.result)) {
                    case RetryOrReset.Reset: ResetEz(state, ez); break;
                    case RetryOrReset.Retry: RetryEz(state, ez); break;
                    default: throw new InvalidOperationException("invalid WhenReqCompletedOpt enum value.");
                }
            }
        }

        public static void FreeMemory<TReqState>(UvAsync async)
        {
            var state = State<TReqState>(async);
            foreach (var kvp in state.Easys) {
                using var _ = kvp.Value;
                Curl.MultiRemoveEz(state.Multi, kvp.Value);
            }

            state.Timer.Stop();
            state.Timer.Dispose();

            foreach (var poll in state.Sockets.Values) {
                using var _ = poll;
                poll.Stop();
            }

            state.Cancel.Dispose();
            state.AddMulti.Dispose();
            state.Disposer.Dispose();
            state.Multi.Dispose();
        }
    }

    public sealed class CurlMultiSocketAgent<TReqState> : IDisposable
    {
        private readonly CurlMultiSocketAgentState<TReqState> _state;
        private bool _disposed;

        public void EnqueueRequest(ReqCtx<TReqState> reqCtx) =>
            Agent.EnqueueRequest(_state, reqCtx);

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;
            _state.Disposer.Send();
        }

        public CurlMultiSocketAgent(int ezHandleCount)
        {
            _state = Agent.Activate<TReqState>(ezHandleCount);
        }
    }
}
