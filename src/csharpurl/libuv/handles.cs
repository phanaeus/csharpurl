using System;
using System.Runtime.InteropServices;
using CSharpUrl.LibC;

namespace CSharpUrl.LibUv
{
    public abstract class UvInitArgs
    {
    }

    public sealed class InitLoopArgs : UvInitArgs
    {
        private InitLoopArgs() { }
        public static InitLoopArgs Value { get; }
        static InitLoopArgs() => Value = new InitLoopArgs();
    }

    public class InitTimerArgs : UvInitArgs
    {
        public IntPtr LoopPtr { get; }

        public InitTimerArgs(IntPtr loopPtr) { LoopPtr = loopPtr; }
    }

    public class InitPollArgs : UvInitArgs
    {
        public IntPtr LoopPtr { get; }
        public int Fd { get; }

        public InitPollArgs(IntPtr loopPtr, int fd)
        {
            LoopPtr = loopPtr;
            Fd = fd;
        }
    }

    public class InitAsyncArgs : UvInitArgs
    {
        public IntPtr LoopPtr { get; }
        public libuv.uv_async_cb Cb { get; }

        public InitAsyncArgs(IntPtr loopPtr, libuv.uv_async_cb cb)
        {
            LoopPtr = loopPtr;
            Cb = cb;
        }
    }

    public abstract class UvMemory : IDisposable, IEquatable<UvMemory>
    {
        private bool _disposed;

        protected IntPtr Handle { get; }

        public object? Data { get; set; }

        private uv_err_code InitUvStruct(
            UvInitArgs args)
        {
            return args switch {
                InitLoopArgs _ => libuv.uv_loop_init(Handle),
                InitTimerArgs timerArgs => libuv.uv_timer_init(timerArgs.LoopPtr, Handle),
                InitPollArgs pollArgs => libuv.uv_poll_init_socket(pollArgs.LoopPtr, Handle, pollArgs.Fd),
                InitAsyncArgs asyncArgs => libuv.uv_async_init(asyncArgs.LoopPtr, Handle, asyncArgs.Cb),
                _ => throw new ArgumentException("unhandled UvInitArgs", nameof(args)),
            };
        }

        private void InitMem(UvInitArgs args)
        {
            var result = InitUvStruct(args);
            UvModule.ValidateResult(result);
        }

        public bool Equals(UvMemory other) => Handle == other.Handle;

        public override string ToString() => $"libuv.{GetType().Name}@{Handle}";

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;
            _disposed = true;
            if (disposing) { }
            Marshal.FreeCoTaskMem(Handle);
            //LibCModule.Free(Handle);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected UvMemory(
            int structSize,
            UvInitArgs args)
        {
            Handle = Marshal.AllocCoTaskMem(structSize);
            //Handle = LibCModule.Calloc(structSize);
            
            try { InitMem(args); }
            catch
            {
                Marshal.FreeCoTaskMem(Handle); throw; 
                //LibCModule.Free(Handle);
                throw;
            }
        }

        ~UvMemory() => Dispose(false);

        public static implicit operator IntPtr(UvMemory mem)
        {
            if (mem._disposed) throw new ObjectDisposedException(mem.GetType().Name);
            return mem.Handle;
        }

        public static unsafe implicit operator void*(UvMemory mem)
        {
            if (mem._disposed) throw new ObjectDisposedException(mem.GetType().Name);
            return (void*)mem.Handle;
        }
    }

    public class UvLoop : UvMemory
    {
        private bool _disposed;

        protected override void Dispose(bool disposing)
        {
            if (_disposed) return;
            _disposed = true;
            if (disposing) { }
            libuv.uv_stop(Handle);
            UvModule.ValidateResult(libuv.uv_loop_close(Handle));
            base.Dispose(true);
        }

        public UvLoop() : base(libuv.uv_loop_size(), InitLoopArgs.Value) { }
    }

    public abstract class UvHandle : UvMemory
    {
        private static readonly libuv.uv_close_cb CloseCb;

        private static void CloseCallback(IntPtr handle)
        {
            var uvHandle = StructData<UvHandle>(handle);
            uvHandle._closeCbOpt?.Invoke(handle);
            var gcHandle = GetGCHandle(handle);
            gcHandle.Free();
            uvHandle.BaseDispose();
        }

        static UvHandle() => CloseCb = CloseCallback;

        private bool _disposed;

        private Action<IntPtr>? _closeCbOpt;

        public uv_handle_type HandleType { get; }

        public UvLoop Loop { get; }

        protected static unsafe GCHandle GetGCHandle(IntPtr handle)
        {
            var ptr = ((uv_handle_t*)handle)->data;
            if (ptr == IntPtr.Zero)
                throw new InvalidOperationException("unexpected null @ data field of uv_handle_t* struct.");

            var gcHandle = GCHandle.FromIntPtr(ptr);
            if (!gcHandle.IsAllocated)
                throw new InvalidOperationException("expected gc handle to be allocated when attempting to get data field from uv_handle_t* struct");
            return gcHandle;
        }

        protected static unsafe T StructData<T>(IntPtr handle) where T : UvHandle
        {
            var gcHandle = GetGCHandle(handle);
            return (T)gcHandle.Target;
        }

        private void BaseDispose() => base.Dispose(true);

        public void Dispose(Action<IntPtr> closeCb)
        {
            _closeCbOpt = closeCb;
            Dispose();
        }

        protected override void Dispose(bool disposing)
        {
            if (_disposed) return;
            _disposed = true;
            if (disposing) { }
            libuv.uv_close(Handle, CloseCb);;
        }

        public override string ToString() => $"{HandleType}@{Handle}";

        protected unsafe UvHandle(
            uv_handle_type handleType,
            UvLoop loop,
            UvInitArgs args) : base(libuv.uv_handle_size(handleType), args)
        {
            HandleType = handleType;
            Loop = loop;
            var gcHandle = GCHandle.Alloc(this);
            var p = (uv_handle_t*)Handle;
            p->data = GCHandle.ToIntPtr(gcHandle);
        }
    }

    public class UvTimer : UvHandle
    {
        private static readonly libuv.uv_timer_cb Cb;

        private static void TimerCallback(IntPtr handle)
        {
            var timer = StructData<UvTimer>(handle);
            timer._cb?.Invoke(timer);
        }

        static UvTimer() => Cb = TimerCallback;

        private Action<UvTimer>? _cb;

        public void Start(Action<UvTimer> callback, long timeout, long repeat)
        {
            _cb = callback;
            UvModule.ValidateResult(
                libuv.uv_timer_start(Handle, Cb, timeout, repeat)
            );
        }

        public void Stop() => UvModule.ValidateResult(libuv.uv_timer_stop(Handle));

        public UvTimer(UvLoop loop) : base(uv_handle_type.UV_TIMER, loop, new InitTimerArgs(loop)) { }
    }

    public class UvPoll : UvHandle
    {
        private static readonly libuv.uv_poll_cb Cb;

        private Action<UvPoll, int, int>? _cb;

        public int Fd { get; }

        public void Start(
            uv_poll_event eventMask,
            Action<UvPoll, int, int> callback)
        {
            _cb = callback;
            UvModule.ValidateResult(libuv.uv_poll_start(Handle, (int)eventMask, Cb));
        }

        public void Stop() => UvModule.ValidateResult(libuv.uv_poll_stop(Handle));

        public UvPoll(UvLoop loop, int fd) : base(uv_handle_type.UV_POLL, loop, new InitPollArgs(loop, fd)) { Fd = fd; }

        private static void PollCallback(IntPtr handle, int status, int events)
        {
            var poll = StructData<UvPoll>(handle);
            poll._cb?.Invoke(poll, status, events);
        }

        static UvPoll() => Cb = PollCallback;
    }

    public class UvAsync : UvHandle
    {
        private static readonly libuv.uv_async_cb Cb;

        private static void AsyncCallback(IntPtr handle)
        {
            var async = StructData<UvAsync>(handle);
            async._cb(async);
        }

        static UvAsync() => Cb = AsyncCallback;

        private readonly Action<UvAsync> _cb;

        public void Send() => UvModule.ValidateResult(libuv.uv_async_send(Handle));

        public UvAsync(UvLoop loop, Action<UvAsync> cb) : base(uv_handle_type.UV_ASYNC, loop, new InitAsyncArgs(loop, Cb)) => _cb = cb;
    }
}
