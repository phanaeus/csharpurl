//#define MEMORYPOOL
using CSharpUrl.LibCurl;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CSharpUrl
{
    // HTTP status codes as per RFC 2616.
    public enum HttpStatusCode
    {
        // Informational 1xx
        Continue = 100,
        SwitchingProtocols = 101,
        Processing = 102,
        EarlyHints = 103,

        // Successful 2xx
        OK = 200,
        Created = 201,
        Accepted = 202,
        NonAuthoritativeInformation = 203,
        NoContent = 204,
        ResetContent = 205,
        PartialContent = 206,
        MultiStatus = 207,
        AlreadyReported = 208,

        IMUsed = 226,

        // Redirection 3xx
        MultipleChoices = 300,
        Ambiguous = 300,
        MovedPermanently = 301,
        Moved = 301,
        Found = 302,
        Redirect = 302,
        SeeOther = 303,
        RedirectMethod = 303,
        NotModified = 304,
        UseProxy = 305,
        Unused = 306,
        TemporaryRedirect = 307,
        RedirectKeepVerb = 307,
        PermanentRedirect = 308,

        // Client Error 4xx
        BadRequest = 400,
        Unauthorized = 401,
        PaymentRequired = 402,
        Forbidden = 403,
        NotFound = 404,
        MethodNotAllowed = 405,
        NotAcceptable = 406,
        ProxyAuthenticationRequired = 407,
        RequestTimeout = 408,
        Conflict = 409,
        Gone = 410,
        LengthRequired = 411,
        PreconditionFailed = 412,
        RequestEntityTooLarge = 413,
        RequestUriTooLong = 414,
        UnsupportedMediaType = 415,
        RequestedRangeNotSatisfiable = 416,
        ExpectationFailed = 417,
        // From the discussion thread on #4382:
        // "It would be a mistake to add it to .NET now. See golang/go#21326,
        // nodejs/node#14644, requests/requests#4238 and aspnet/HttpAbstractions#915".
        ImATeapot = 418,
        Dank = 420,
        MisdirectedRequest = 421,
        UnprocessableEntity = 422,
        Locked = 423,
        FailedDependency = 424,

        UpgradeRequired = 426,

        PreconditionRequired = 428,
        TooManyRequests = 429,

        RequestHeaderFieldsTooLarge = 431,

        UnavailableForLegalReasons = 451,

        // Server Error 5xx
        InternalServerError = 500,
        NotImplemented = 501,
        BadGateway = 502,
        ServiceUnavailable = 503,
        GatewayTimeout = 504,
        HttpVersionNotSupported = 505,
        VariantAlsoNegotiates = 506,
        InsufficientStorage = 507,
        LoopDetected = 508,

        NotExtended = 510,
        NetworkAuthenticationRequired = 511
    }

    internal static class Utils
    {
        public static bool InvariantEquals(string str1, string str2) =>
            string.Equals(str1, str2, StringComparison.OrdinalIgnoreCase);

        public static bool InvariantStartsWith(string input, string value) =>
            input.StartsWith(value, StringComparison.OrdinalIgnoreCase);

        public static unsafe string ConvertSpanToStr(in ReadOnlySpan<byte> bytes)
        {
            fixed (byte* b = bytes)
                return Encoding.UTF8.GetString(b, bytes.Length);
        }

        public static string[] SplitRemoveEmpty(string input, char del)
            => input.Split(new[] { del }, StringSplitOptions.RemoveEmptyEntries);

        public static string[] SplitRemoveEmpty(string input, char[] del)
            => input.Split(del, StringSplitOptions.RemoveEmptyEntries);

        public static (bool success, (string key, string val)) TryParseKvp(string input, char[] delimter)
        {
            if (string.IsNullOrWhiteSpace(input))
                return default!;

            var sp = SplitRemoveEmpty(input, delimter);
            return sp.Length < 2 ? default! : (true, (sp[0], string.Join(delimter.ToString(), sp.Skip(1))));
        }

        public static (bool success, ArraySegment<T> arrSeg) TryGetArraySegment<T>(in ReadOnlyMemory<T> memory) =>
            MemoryMarshal.TryGetArray(memory, out var segment) ? (true, segment) : default!;

        public static ArraySegment<T> AsArraySeg<T>(in ReadOnlyMemory<T> memory)
        {
            var (success, seg) = TryGetArraySegment(memory);
            if (!success)
                throw new InvalidOperationException("memory did not contain an array.");
            return seg;
        }

#if NETSTANDARD2_0
        public static void Write(this Stream stream, in ReadOnlySpan<byte> bytes)
        {
            using var mem = MemoryPool<byte>.Shared.Rent(bytes.Length);
            bytes.CopyTo(mem.Memory.Span);
            ReadOnlyMemory<byte> buffer = mem.Memory.Slice(0, bytes.Length);
            var arraySeg = AsArraySeg(buffer);
            stream.Write(arraySeg.Array, arraySeg.Offset, arraySeg.Count);
        }

        public static void Write(this Stream stream, in ReadOnlyMemory<byte> bytes)
        {
            var seg = AsArraySeg(bytes);
            stream.Write(seg.Array, seg.Offset, seg.Count);
        }
#endif

        public static (bool success, T value) TryFind<T>(IEnumerable<T> seq, Func<T, bool> predicate)
        {
            foreach (var item in seq) {
                if (predicate(item))
                    return (true, item);
            }

            return default!;
        }
    }

    public static class HttpUtils
    {
        private const string UnreservedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.~";

        /// <summary>
        /// Url encodes input
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string UrlEncode(string input)
        {
            var sb = new StringBuilder(input.Length + 23);
            void Append(char ch)
            {
                if (UnreservedChars.IndexOf(ch) == -1) 
                    sb.AppendFormat("%{0:X2}", (int)ch);
                else 
                    sb.Append(ch);
            }
            foreach (var ch in input)
                Append(ch);
            return sb.ToString();
        }

        public static string UrlEncodeKvp((string key, string val) kvp)
        {
            var (key, val) = kvp;
            return $"{UrlEncode(key)}={UrlEncode(val)}";
        }


        /// <summary>
        /// Url encodes sequence of <see cref="ValueTuple"/>s;.
        /// </summary>
        /// <param name="sequence"></param>
        /// <returns></returns>
        public static string UrlEncodeSeq(IEnumerable<(string key, string value)> sequence) =>
            string.Join("&", sequence.Select(UrlEncodeKvp));
    }

    public class Cookie : IEquatable<Cookie>
    {
        public string Name { get; }
        public string Value { get; }
        public string Path { get; }
        public string Domain { get; }
        public DateTimeOffset Expires { get; }
        public bool HttpOnly { get; }
        public bool Secure { get; }

        public Cookie(
            string name,
            string value,
            string path = "/",
            string domain = "*",
            DateTimeOffset expires = default,
            bool httpOnly = true,
            bool secure = false)
        {
            Name = name;
            Value = value;
            Path = path;
            Domain = domain;
            Expires = expires;
            HttpOnly = httpOnly;
            Secure = secure;
        }

        public override int GetHashCode()
        {
            unchecked {
                var hash = 23;
                hash = (hash * 23) + Name.GetHashCode();
                hash = (hash * 23) + Value.GetHashCode();
                hash = (hash * 23) + Domain.GetHashCode();
                hash = (hash * 23) + Path.GetHashCode();
                return (hash * 23) + Expires.GetHashCode();
            }
        }

        public bool Equals(Cookie other)
        {
            if (other == null)
                return false;
            return Name == other.Name && Value == other.Value && Path == other.Path && Domain == other.Domain && Expires == other.Expires && Secure == other.Secure && HttpOnly == other.HttpOnly;
        }

        public override string ToString() => $"{Name}={Value}";

        private static string RemoveNameValueIfExists(string input)
        {
            return Utils.InvariantStartsWith(input, "Set-Cookie:") ? input.Substring(11).Trim() : input;
        }

        private static (bool success, (string key, string val)) TryParseCookieNvp(string input)
        {
            var s = input.Split(new[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
            if (s.Length < 2)
                return default;
            var key = s[0];
            var val = string.Concat(s.Skip(1).Take(s.Length - 1));
            return (true, (key, /*Uri.UnescapeDataString*/(val)));
        }

        private static Cookie ParseAttribs(string name, string value, string defDomain, IEnumerable<string> attributes)
        {
            var (expires, domain, path, secure, httpOnly) =
                (DateTimeOffset.MaxValue, defDomain, "/", false, false);

            foreach (var attrib in attributes) {
                if (Utils.InvariantStartsWith(attrib, "expires")) {
                    var (success, (k, v)) = TryParseCookieNvp(attrib);
                    if (success && DateTimeOffset.TryParse(v, out var exp))
                        expires = exp;
                } else if (Utils.InvariantStartsWith(attrib, "domain")) {
                    var (success, (k, v)) = TryParseCookieNvp(attrib);
                    if (success)
                        domain = v;
                } else if (Utils.InvariantStartsWith(attrib, "path")) {
                    var (success, (k, v)) = TryParseCookieNvp(attrib);
                    if (success)
                        path = v;
                } else if (Utils.InvariantStartsWith(attrib, "secure")) {
                    secure = true;
                } else if (Utils.InvariantStartsWith(attrib, "httponly")) {
                    httpOnly = true;
                }
            }

            return new Cookie(name, value, path, domain, expires, httpOnly, secure);
        }

        public static (bool success, Cookie cookie) TryParse(string input, string defaultDomain)
        {
            if (string.IsNullOrWhiteSpace(input))
                return default;

            var valueText = RemoveNameValueIfExists(input);
            var cookiePieces = valueText.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            var nameValStr = cookiePieces[0];
            var (success, (name, value)) = TryParseCookieNvp(nameValStr);
            if (!success)
                return default;

            if (cookiePieces.Length >= 3) {
                var attribs =
                    cookiePieces
                    .Skip(1)
                    .Select(s => s.Trim());
                return (true, ParseAttribs(name, value, defaultDomain, attribs));
            } else {
                return (true, new Cookie(
                    name,
                    value,
                    "/",
                    defaultDomain,
                    DateTimeOffset.MaxValue,
                    false,
                    false)
                );
            }
        }
    }

    public abstract class HttpContent
        : IDisposable
    {
        private bool _disposed;
        //private readonly IMemoryOwner<byte> _memOwner;

        public ReadOnlyMemory<byte> Content { get; }

        protected HttpContent(
            //in ReadOnlySpan<byte> content
            //byte[] content
            in ReadOnlyMemory<byte> content
        )
        {
            //_memOwner = MemoryPool<byte>.Shared.Rent(content.Length);
            //content.CopyTo(_memOwner.Memory.Span);
            //Content = _memOwner.Memory.Slice(0, content.Length);
            Content = content;
        }

        public override string ToString() => Utils.ConvertSpanToStr(Content.Span);

#pragma warning disable CA1063 // Implement IDisposable Correctly. NO
        public void Dispose()
#pragma warning restore CA1063 // Implement IDisposable Correctly
        {
            if (_disposed) return;
            _disposed = true;
            //_memOwner.Dispose();
        }
    }

    public class ReadOnlyMemoryHttpContent : HttpContent
    {
        public ReadOnlyMemoryHttpContent(
            //in ReadOnlySpan<byte> content
            in ReadOnlyMemory<byte> content
        ) : base(content) { }
        public override string ToString() => Content.ToString();
    }

    public class StringHttpContent : HttpContent
    {
        private readonly string? _str;
        private StringHttpContent(
            // in ReadOnlySpan<byte> content
            in ReadOnlyMemory<byte> content
        ) : base(content) { }

        public StringHttpContent(string str) : this(Encoding.UTF8.GetBytes(str)) 
        {
            _str = str;
        }
        
        public override string ToString() => _str!;
    }
    
    public class EncodedFormValuesHttpContent : HttpContent
    {
        private readonly IEnumerable<(string, string)>? _seq;
        private EncodedFormValuesHttpContent(
             // in ReadOnlySpan<byte> content
             in ReadOnlyMemory<byte> content
        ) : base(content) { }
        public EncodedFormValuesHttpContent(IEnumerable<(string, string)> sequence) : this(Encoding.UTF8.GetBytes(HttpUtils.UrlEncodeSeq(sequence))) { _seq = sequence; }
        public override string ToString() => HttpUtils.UrlEncodeSeq(_seq!);
    }

    public static class HttpVersion
    {
        public static Version Http11 { get; }
        public static Version Http2 { get; }

        static HttpVersion()
        {
            Http11 = new Version("1.1");
            Http2 = new Version("2.0");
        }
    }

    public enum IpResolve
    {
        Default,
        IPV4,
        IPV6
    }

    /// <summary>
    /// Contains http request information
    /// </summary>
    public class HttpReq
    {
        public string HttpMethod { get; }
        public Uri Uri { get; }
        public IEnumerable<(string key, string val)> Headers { get; set; } = new List<(string key, string val)>();
        public HttpContent? ContentBody { get; set; }
        public Proxy? Proxy { get; set; }
        public TimeSpan Timeout { get; set; } = System.Threading.Timeout.InfiniteTimeSpan;
        public IEnumerable<Cookie> Cookies { get; set; } = new List<Cookie>();
        public bool AutoRedirect { get; set; } = true;
        public Version ProtocolVersion { get; set; } = HttpVersion.Http11;
        public bool KeepAlive { get; set; } = true;
        public int MaxRetries { get; set; } = 3;
        public bool Insecure { get; set; } = false;
        public bool Verbose { get; set; } = false;
        public IpResolve IpResolve { get; set; } = IpResolve.Default;

        public HttpReq(string method, Uri uri)
        {
            if (!Utils.InvariantStartsWith(uri.Scheme, "http"))
                throw new ArgumentException($"{uri} is not a http/https uri.", nameof(uri));

            HttpMethod = method;
            Uri = uri;
        }

        /// <exception cref="ArgumentException"/>
        public HttpReq(
            string method,
            string uri)
        {
            if (!Uri.TryCreate(uri, UriKind.Absolute, out var ruri))
                throw new ArgumentException($"{uri} is not a valid Uri.", nameof(uri));
            if (!Utils.InvariantStartsWith(ruri.Scheme, "http"))
                throw new ArgumentException($"{uri} is not a http/https uri.", nameof(uri));
            HttpMethod = method;
            Uri = ruri;
        }

        public override string ToString()
        {
            var proxyStr = Proxy == null ? "None" : Proxy.ToString();
            return $"method: {HttpMethod} uri: {Uri} protocol: HTTP/{ProtocolVersion.ToString()} proxy: {proxyStr}";
        }
    }


    /// <summary>
    /// Contains http response information.
    /// </summary>
    [DebuggerDisplay("status_code: {StatusCode} uri: {Uri.ToString(),nq} content-length: {ContentData.Length}")]
    public class HttpResp : IDisposable
    {
//#if MEMORYPOOL
//        private readonly IMemoryOwner<byte> _owner;
//#endif
        private bool _disposed;
        private string _copy;
        private readonly MemoryStream? _contentStream;

        public HttpStatusCode StatusCode { get; }
        public Uri Uri { get; }
        public ReadOnlyDictionary<string, List<string>> Headers { get; }
        public ReadOnlyCollection<Cookie> Cookies { get; }
        public ReadOnlyMemory<byte> ContentData { get; }
        public HttpReq CorrespondingRequest { get; }

        public string Content
        {
            get {
                if (_disposed)
                    throw new ObjectDisposedException(GetType().Name);

                if (ContentData.Length == 0)
                    return "";

                if (string.IsNullOrEmpty(_copy))
                    _copy = Utils.ConvertSpanToStr(ContentData.Span);

                return _copy;
            }
        }

        internal HttpResp(
            HttpStatusCode statusCode,
            Uri uri,
            ReadOnlyDictionary<string, List<string>> headers,
            ReadOnlyCollection<Cookie> cookies,
            MemoryStream content,
            HttpReq req)
        {
            _copy = string.Empty;
            StatusCode = statusCode;
            Uri = uri;
            Headers = headers;
            Cookies = cookies;

            //_owner = MemoryPool<byte>.Shared.Rent(contentData.Length);
            //contentData.CopyTo(_owner.Memory.Span);
            //ContentData = _owner.Memory.Slice(0, contentData.Length);
            _contentStream = content;
            var mem = new ReadOnlyMemory<byte>(content.GetBuffer());
            ContentData = mem.Slice(0, (int)content.Position);
            CorrespondingRequest = req;
        }

        public HttpResp(
            HttpStatusCode statusCode,
            Uri uri,
            ReadOnlyDictionary<string, List<string>> headers,
            ReadOnlyCollection<Cookie> cookies,
            //in ReadOnlySpan<byte> contentData
            in ReadOnlyMemory<byte> contentData,
            HttpReq req)
        {
            _copy = string.Empty;
            StatusCode = statusCode;
            Uri = uri;
            Headers = headers;
            Cookies = cookies;

            //_owner = MemoryPool<byte>.Shared.Rent(contentData.Length);
            //contentData.CopyTo(_owner.Memory.Span);
            //ContentData = _owner.Memory.Slice(0, contentData.Length);
            ContentData = contentData;
            CorrespondingRequest = req;
        }

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;
            _contentStream?.Dispose();
            //_owner.Dispose();
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append(StatusCode).Append(" - ").Append(Uri).AppendLine();
            foreach (var kvp in Headers)
                sb.Append(kvp.Key).Append(": ").Append(string.Join(", ", kvp.Value)).AppendLine();

            return sb.ToString();
        }
    }


    internal class HttpStatusInfo
    {
        public Version Version { get; }
        public HttpStatusCode StatusCode { get; }

        public HttpStatusInfo(Version version, HttpStatusCode statusCode)
        {
            Version = version;
            StatusCode = statusCode;
        }

        private static string VersionStr(string input)
        {
            return input == "2" ? "2.0" : input;
        }

        public override string ToString()
        {
            return $"HTTP/{Version} {StatusCode}";
        }

        private static readonly char[] Space = new char[] { ' ' };
        private static readonly char[] Slash = new char[] { '/' };

        public static (bool success, HttpStatusInfo? info) TryParse(string input)
        {
            if (!input.StartsWith("http", StringComparison.OrdinalIgnoreCase))
                return default;
            var words = input.Split(Space, StringSplitOptions.RemoveEmptyEntries);
            if (words.Length <= 1)
                return default;
            var httpVer = words[0].Split(Slash, StringSplitOptions.RemoveEmptyEntries);
            if (httpVer.Length != 2)
                return default;
            var versionStr = VersionStr(httpVer[1]);
            if (!Version.TryParse(versionStr, out var version))
                return default;
            var statusStr = words[1];
            return !Enum.TryParse<HttpStatusCode>(statusStr, out var httpStatCode) ? default : (true, new HttpStatusInfo(version, httpStatCode));
        }
    }

    internal enum HttpRecvState
    {
        Status,
        Headers,
        Body
    }

    internal static class HttpReqStateModule
    {
        private static readonly ReadOnlyMemory<byte> CrLf = new byte[] { (byte)'\r', (byte)'\n' };
        private static readonly char[] Colon = new char[] { ':' };

        private static void TryParseHeaderLine(HttpReqState state, string input)
        {
            var (parsedStat, statInfo) = HttpStatusInfo.TryParse(input);
            if (parsedStat) // statInfo != null kaceyUp
            {
                state.Statuses.Add(statInfo!);
                state.RecvState = HttpRecvState.Headers;
                return;
            }

            var (parsedHeader, (header, value)) = Utils.TryParseKvp(input, Colon);
            if (!parsedHeader) {
#if DEBUG
                Console.Error.WriteLine($"Warning: Failed to parse http header with input: {input}");
#endif
                return;
            }

            if (!state.Headers.ContainsKey(header))
                state.Headers[header] = new List<string>(1);
            state.Headers[header].Add(value.Trim());
        }

        public static CurlSlist CreateSList(HttpReq req)
        {
            var slist = new CurlSlist();
            libcurl.curl_slist_append(slist, "Expect:");
            libcurl.curl_slist_append(slist, "Accept:");
            foreach (var (key, val) in req.Headers)
                libcurl.curl_slist_append(slist, $"{key}: {val}");
            return slist;
        }

        private static unsafe int ValidateCurlWriteCallbackParams(int size, int nitems, void* userData)
        {
            if (size != 1) {
                throw new InvalidOperationException(
                    "Curl write callback error: expect a single array."
                );
            }

            var len = size * nitems;
            
            if (len < 0) {
                throw new InvalidOperationException(
                    "Curl write callback error: callback invoked with array length <= 0."
                );
            }

            if (userData == null) {
                throw new InvalidOperationException(
                    "Curl write callback error: callback invoked with null user data."
                );
            }

            return len;
        }

        private static unsafe HttpReqState PtrToReqState(void* userData)
        {
            var handle = GCHandle.FromIntPtr((IntPtr)userData);
            if (!handle.IsAllocated)
                throw new InvalidOperationException("gc handle not allocated.");
            if (!(handle.Target is HttpReqState state)) {
                throw new InvalidOperationException(
                    "Curl write callback error: callback invoked with invalid user data pointer."
                );
            }
            return state;
        }
        
        public static unsafe int HandleHeaderLine(byte* ptr, int size, int nitems, void* userData)
        {
            var len = ValidateCurlWriteCallbackParams(size, nitems, userData);
            if (len == 0)
                return 0;

            var state = PtrToReqState(userData);

            /* It's important to note that the callback will be invoked for the headers of all responses received after
               initiating a request and not just the final response. This includes all responses which occur during
               authentication negotiation. If you need to operate on only the headers from the final response, you will
               need to collect headers in the callback yourself and use HTTP status lines, for example, to delimit
               response boundaries. */
            if (state.RecvState == HttpRecvState.Body)
                state.RecvState = HttpRecvState.Status;

            var bytes = new ReadOnlySpan<byte>(ptr, len);
            if (bytes.SequenceEqual(CrLf.Span)) {
                // end of http headers
                state.RecvState = HttpRecvState.Body;
                return len;
            }
            
            var str = Utils.ConvertSpanToStr(bytes);
            TryParseHeaderLine(state, str);

            return len;
        }

        public static unsafe int HandleContentData(byte* ptr, int size, int nitems, void* userData)
        {
            var len = ValidateCurlWriteCallbackParams(size, nitems, userData);
            if (len == 0)
                return 0;

#if MEMORYPOOL
            using var mem = MemoryPool<byte>.Shared.Rent(len);
            var buffer = mem.Memory.Slice(0, len);
            var span = new Span<byte>(ptr, len);
            span.CopyTo(buffer.Span);
#else
            var buffer = new byte[len];
            for (var i = 0; i < len; i++)
                buffer[i] = ptr[i];
#endif

            var state = PtrToReqState(userData);

            if (state.MemStrem == null) {
                state.MemStrem = new MemoryStream(buffer.Length);
                    //HttpModule.MemeoryManager.GetStream(
                    //    ((IntPtr)state.EzHandle!).ToString(),
                    //    buffer.Length
                    //);
            }


#if MEMORYPOOL
            state.MemStrem.Write(buffer);
#else
            state.MemStrem.Write(buffer, 0, buffer.Length);
#endif
            // state.MemStrem.Write(data);
            return len;
        }
    }

    internal class HttpReqState : IDisposable
    {
        private static readonly libcurl.unsafe_write_callback _contentCallback;
        private static readonly libcurl.unsafe_write_callback _headerCallback;

        static unsafe HttpReqState()
        {
            _headerCallback = HttpReqStateModule.HandleHeaderLine;
            _contentCallback = HttpReqStateModule.HandleContentData;
        }

        private GCHandle _hndl;

        private bool _disposed;

        public HttpReq Req { get; }
        public unsafe libcurl.unsafe_write_callback HeaderDataHandler { get; }
        public unsafe libcurl.unsafe_write_callback ContentDataHandler { get; }
        public Dictionary<string, List<string>> Headers { get; }
        public CurlSlist HeadersSlist { get; }
        public List<HttpStatusInfo> Statuses { get; }
        public TaskCompletionSource<HttpResp> Tcs { get; }
        public int Redirects { get; set; }
        public HttpRecvState RecvState { get; set; }
        public MemoryStream? MemStrem { get; set; }
        public int Attempts { get; set; }
        public CurlEzHandle? EzHandle { get; set; } = default;

        public ReadOnlyMemory<byte> Content
        {
            get {
                if (_disposed)
                    throw new ObjectDisposedException(GetType().Name);

                if (MemStrem == null)
                    return ReadOnlyMemory<byte>.Empty;

                var copy = MemStrem.ToArray();
                return new ReadOnlyMemory<byte>(copy);
            }
        }

        public IntPtr Handle
        {
            get {
                if (_disposed)
                    throw new ObjectDisposedException(GetType().Name);
                return GCHandle.ToIntPtr(_hndl);
            }
        }

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;
            //MemStrem?.Dispose();
            HeadersSlist.Dispose();
            _hndl.Free();
            
            ;
        }


        public HttpReqState(HttpReq req)
        {
            _hndl = GCHandle.Alloc(this);
            Req = req;
            HeaderDataHandler = _headerCallback;
            ContentDataHandler = _contentCallback;
            HeadersSlist = HttpReqStateModule.CreateSList(req);
            Statuses = new List<HttpStatusInfo>(2);
            Tcs = new TaskCompletionSource<HttpResp>();
            Headers = new Dictionary<string, List<string>>(7, StringComparer.OrdinalIgnoreCase);
        }
    }

    public static class HttpModule
    {
        private const int HttpVersion11 = 2;
        private const int HttpVersion20 = 4;

        private static readonly int MaxCurlAgents =
#if DEBUG
            1;
#else
            2;
#endif


        private static readonly Queue<CurlMultiSocketAgent<HttpReqState>> Agents;
        //internal static readonly RecyclableMemoryStreamManager MemeoryManager;

        static HttpModule()
        {
            CURLcode initResult;
            try {
                initResult = libcurl.curl_global_init();
            }
            catch (DllNotFoundException e) {
                Environment.FailFast($"{e}", e);
                throw;
            }

            if (initResult != CURLcode.OK)
                Environment.FailFast($"curl_global_init returned {initResult} ~ {libcurl.curl_easy_strerror(initResult)}");

            var q = new Queue<CurlMultiSocketAgent<HttpReqState>>(MaxCurlAgents);
            for (var i = 0; i < MaxCurlAgents; i++) {
                var agent = new CurlMultiSocketAgent<HttpReqState>(144);
                q.Enqueue(agent);
            }

            Agents = q;
            // MemeoryManager = new RecyclableMemoryStreamManager(377, 87841, 87841);
            CACertInfo.Init();
        }

        private static void SetEzOpt(IntPtr handle, CURLoption opt, string value) =>
            Curl.ValidateSetOptResult(libcurl.curl_easy_setopt(handle, opt, value));

        private static void SetEzOpt(IntPtr handle, CURLoption opt, int value) =>
            Curl.ValidateSetOptResult(libcurl.curl_easy_setopt(handle, opt, value));

        private static void SetEzOpt(IntPtr handle, CURLoption opt, libcurl.unsafe_write_callback value) =>
            Curl.ValidateSetOptResult(libcurl.curl_easy_setopt(handle, opt, value));

        private static void SetEzOpt(IntPtr handle, CURLoption opt, IntPtr value) =>
            Curl.ValidateSetOptResult(libcurl.curl_easy_setopt(handle, opt, value));

        private static void SetEzOpt(IntPtr handle, CURLoption opt, byte[] value) =>
            Curl.ValidateSetOptResult(libcurl.curl_easy_setopt(handle, opt, value));

        /*
         Strings passed to libcurl as 'char *' arguments, are copied by the library; thus the string storage associated to the pointer 
         argument may be overwritten after curl_easy_setopt returns. The only exception to this rule is really CURLOPT_POSTFIELDS, 
         but the alternative that copies the string CURLOPT_COPYPOSTFIELDS has some usage characteristics you need to read up on. 
        */
        private static void ConfigureEz(CurlEzHandle ez, HttpReqState state)
        {
            try {
                if (state.EzHandle == default)
                    state.EzHandle = ez;
                var httpReq = state.Req;
                SetEzOpt(ez, CURLoption.CUSTOMREQUEST, httpReq.HttpMethod);
                SetEzOpt(ez, CURLoption.URL, httpReq.Uri.ToString());

                if (httpReq.Timeout.TotalMilliseconds > 0)
                    SetEzOpt(ez, CURLoption.TIMEOUT_MS, (int)httpReq.Timeout.TotalMilliseconds);

                SetEzOpt(ez, CURLoption.HEADERFUNCTION, state.HeaderDataHandler);
                SetEzOpt(ez, CURLoption.HEADERDATA, state.Handle);
                SetEzOpt(ez, CURLoption.WRITEFUNCTION, state.ContentDataHandler);
                SetEzOpt(ez, CURLoption.WRITEDATA, state.Handle);
                SetEzOpt(ez, CURLoption.CAINFO, CACertInfo.CAPem.FullName);

#if DEBUG
                SetEzOpt(ez, CURLoption.SSL_VERIFYPEER, 0);
#else
                if (httpReq.Insecure)
                    SetEzOpt(ez, CURLoption.SSL_VERIFYPEER, 0);
#endif
                if (httpReq.Verbose)
                    SetEzOpt(ez, CURLoption.VERBOSE, 1);

                SetEzOpt(ez, CURLoption.IPRESOLVE, (int)httpReq.IpResolve);

                var (success, (_, acceptEncoding)) = Utils.TryFind(
                    httpReq.Headers,
                    valueTup => {
                        var (key, _) = valueTup;
                        return Utils.InvariantEquals(key, "accept-encoding");
                    }

                );

                if (success)
                    SetEzOpt(ez, CURLoption.ACCEPT_ENCODING, acceptEncoding);

                SetEzOpt(ez, CURLoption.HTTPHEADER, state.HeadersSlist);
                SetEzOpt(ez, CURLoption.COOKIEFILE, "");

                if (httpReq.Cookies.Any()) {
                    var cookiesStr = string.Join("; ", httpReq.Cookies.Select(c => c.ToString()));
                    SetEzOpt(ez, CURLoption.COOKIE, cookiesStr);
                }

                void SetProxy(Proxy proxy)
                {
                    SetEzOpt(ez, CURLoption.PROXY, proxy.Uri.ToString());
                    if (proxy.Credentials != null)
                        SetEzOpt(ez, CURLoption.PROXYUSERPWD, proxy.Credentials.ToString());
                }

                if (httpReq.Proxy != null)
                    SetProxy(httpReq.Proxy);

                if (httpReq.ProtocolVersion.Major == 1)
                    SetEzOpt(ez, CURLoption.HTTP_VERSION, HttpVersion11);
                else if (httpReq.ProtocolVersion.Major == 2) 
                    SetEzOpt(ez, CURLoption.HTTP_VERSION, HttpVersion20);

                //SetEzOpt(ez, CURLoption.MAXCONNECTS, 1);
                
                void SetContent(HttpContent content)
                {
                    var bytes = Utils.AsArraySeg(content.Content);
                    SetEzOpt(ez, CURLoption.POSTFIELDSIZE, bytes.Count);
                    SetEzOpt(ez, CURLoption.COPYPOSTFIELDS, bytes.Array);
                }

                if (!httpReq.KeepAlive)
                    SetEzOpt(ez, CURLoption.FORBID_REUSE, 1);

                if (httpReq.ContentBody != null)
                    SetContent(httpReq.ContentBody);
            }
            catch (InvalidOperationException e) {
                state.Tcs.SetException(e);
            }
        }

        private static void ParseEz(CurlEzHandle ez, HttpReqState state)
        {
            Curl.ValidateGetInfoResult(
                libcurl.curl_easy_getinfo(ez, CURLINFO.EFFECTIVE_URL, out IntPtr ptr)
            );

            var uriStr = Marshal.PtrToStringAnsi(ptr);
            if (string.IsNullOrWhiteSpace(uriStr))
                throw new CurlException("curl returned NULL uri string.");

            if (!Uri.TryCreate(uriStr, UriKind.Absolute, out var uri))
                throw new InvalidOperationException("failed to parse uri from curl easy.");

            var cookz = new List<Cookie>();
            if (state.Headers.ContainsKey("set-cookie")) {
                foreach (var cook in state.Headers["set-cookie"]) {
                    var (success, cookie) = Cookie.TryParse(cook, $".{uri.Authority}");
                    if (success)
                        cookz.Add(cookie);
                }
            }

            //var contentMem = state.Content;
            //var resp = new HttpResp(
            //    state.Statuses[state.Statuses.Count - 1].StatusCode,
            //    uri,
            //    new ReadOnlyDictionary<string, List<string>>(state.Headers),
            //    new ReadOnlyCollection<Cookie>(cookz),
            //    //contentMem.Span
            //    contentMem,
            //    state.Req
            //);

            if (state.MemStrem == null)
                state.MemStrem = new MemoryStream(0);

            var resp = new HttpResp(
                state.Statuses.Last().StatusCode,
                uri,
                new ReadOnlyDictionary<string, List<string>>(state.Headers),
                new ReadOnlyCollection<Cookie>(cookz),
                state.MemStrem!,
                state.Req
            );
            state.Tcs.SetResult(resp);
        }

        private static bool ShouldFollowRedirect(HttpReqState state) =>
             state.Req.AutoRedirect
             && (int)state.Statuses.Last().StatusCode / 100 == 3
             && state.Redirects++ < 10;

        private static void RedirectEz(CurlEzHandle ez)
        {
            Curl.ValidateGetInfoResult(
                libcurl.curl_easy_getinfo(ez, CURLINFO.REDIRECT_URL, out IntPtr urlPtr)
            );
            if (urlPtr == IntPtr.Zero) 
                throw new CurlException("http server returned redirect status code with no redirect uri.");

            var redirect = Marshal.PtrToStringAnsi(urlPtr);
            Curl.ValidateSetOptResult(libcurl.curl_easy_setopt(ez, CURLoption.URL, redirect));
        }

        private static RetryOrReset ResetOrRetry(
            CurlEzHandle ez,
            HttpReqState state)
        {
            if (state.Statuses.Count == 0)
                throw new CurlException("malformed http response received. no status info parsed.");

            if (!ShouldFollowRedirect(state)) {
                ParseEz(ez, state);
                return RetryOrReset.Reset;
            }

            RedirectEz(ez);
            return RetryOrReset.Retry;
        }

        private static string ErrMsg(CURLcode result, HttpReq req) =>
            $"{result} error occored. ~ {libcurl.curl_easy_strerror(result)}. proxy={req.Proxy?.ToString() ?? "None"} method={req.HttpMethod} uri={req.Uri} version={req.ProtocolVersion}.";

        private static RetryOrReset Proc(CURLcode result, CurlEzHandle ez, HttpReqState state) =>
            result switch {
                CURLcode.OK => ResetOrRetry(ez, state),
                _ when ++state.Attempts < state.Req.MaxRetries => RetryOrReset.Retry,
                _ => throw new CurlECodeException(ErrMsg(result, state.Req), result)
            };


        private static RetryOrReset ProcessResult(CurlEzHandle ez, HttpReqState state, CURLcode result)
        {
            try {
                return Proc(result, ez, state);
            }
            catch (InvalidOperationException e) {
                state.Tcs.SetException(e);
                return RetryOrReset.Reset;
            }
        }

        private static void Cancel(HttpReqState state) => state.Tcs.SetCanceled();
        
        private static CurlMultiSocketAgent<HttpReqState> NextCurlMultiAgent()
        {
            lock (Agents) {
                var agent = Agents.Dequeue();
                Agents.Enqueue(agent);
                return agent;
            }
        }

        public static async Task<HttpResp> RetrRespAsync(HttpReq req, CancellationToken cancellationToken)
        {
            using var state = new HttpReqState(req);
            try {
                var reqCtx = new ReqCtx<HttpReqState>(
                    state,
                    ConfigureEz,
                    ProcessResult,
                    Cancel,
                    cancellationToken
                );
                var agent = NextCurlMultiAgent();
                agent.EnqueueRequest(reqCtx);
                return await state.Tcs.Task.ConfigureAwait(false);
            } catch (Exception e) when (e is InvalidOperationException || e is OperationCanceledException) {
                state.MemStrem?.Dispose();
                throw;
            }
        }

        public static async Task<HttpResp> RetrRespAsync(HttpReq req)
        {
            return await RetrRespAsync(req, CancellationToken.None).ConfigureAwait(false);
        }
    }
}
