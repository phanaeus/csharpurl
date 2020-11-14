using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using CSharpUrl.LibC;

namespace CSharpUrl.LibCurl
{
    public enum NullPtrPolicy
    {
        Allowed,
        NotAllowed
    }

    public abstract class CurlMemory : IEquatable<CurlMemory>, IDisposable
    {
        private bool _disposed;

        protected IntPtr Handle { get; set; }

        protected abstract void Delete();

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;
            _disposed = true;
            if (disposing) { }
            Delete();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public bool Equals(CurlMemory other)
        {
            return other != default && Handle == other.Handle;
        }

        public override string ToString() => $"{GetType().Name}@{Handle}";

        protected CurlMemory(in IntPtr handle, NullPtrPolicy nullPtrPolicy)
        {
            if (nullPtrPolicy == NullPtrPolicy.NotAllowed && handle == IntPtr.Zero)
                throw new ArgumentException("cannot init with NULL handle.", nameof(handle));
            Handle = handle;
        }

        ~CurlMemory() => Dispose(false);

        public static implicit operator IntPtr(CurlMemory mem)
        {
            if (mem._disposed) throw new ObjectDisposedException(mem.GetType().Name);
            return mem.Handle;
        }

        public static unsafe implicit operator void* (CurlMemory mem)
        {
            if (mem._disposed) throw new ObjectDisposedException(mem.GetType().Name);
            return (void*)mem.Handle;
        }
    }

    public class CurlEzHandle : CurlMemory
    {
        internal CurlEzHandle(in IntPtr handle) : base(handle, NullPtrPolicy.NotAllowed) { }

        protected override void Delete() => libcurl.curl_easy_cleanup(Handle);
    }

    public class CurlMultiHandle : CurlMemory
    {
        internal CurlMultiHandle(in IntPtr handle) : base(handle, NullPtrPolicy.NotAllowed) { }

        protected override void Delete()
        {
            var result = libcurl.curl_multi_cleanup(Handle);
            if (result != CURLMcode.OK)
                throw new CurlMultiECodeException($"libcurl.curl_multi_cleanup returned error: {result}", result);
        }
    }

    public class CurlSlist : CurlMemory
    {
        public CurlSlist() : base(IntPtr.Zero, NullPtrPolicy.Allowed) { }

        protected override void Delete()
        {
            if (Handle == IntPtr.Zero) return;
            libcurl.curl_slist_free_all(Handle);
        }

        internal void SetHandle(in IntPtr handle) => Handle = handle;
    }

//    internal static class CURIModule
//    {
//        
////        public static unsafe (IntPtr, string?) TryGetUriPart(IntPtr curluHandle, CURLUPart part, CURLUflags flags)
////        {
////            byte* freeMe;
////            var result = libcurl.curl_url_get((void*) curluHandle, part, &freeMe, (ulong) flags);
////            if (result != CURLUcode.CURLUE_OK && freeMe != null) throw new InvalidOperationException("dude wait what");
////            if (result != CURLUcode.CURLUE_OK)
////                return default;
////            
////            var str = new string((sbyte*)freeMe);
////            return (new IntPtr(freeMe), str);
////        }
//
//        public static unsafe IntPtr UriPart(IntPtr curluHandle, CURLUPart part, CURLUflags flags)
//        {
//            byte* freeMe;
//            var result = libcurl.curl_url_get((void*) curluHandle, part, &freeMe, (ulong) flags);
//            if (result != CURLUcode.CURLUE_OK && freeMe != null) 
//                throw new InvalidOperationException("dude wait what");
//            if (result != CURLUcode.CURLUE_OK) {
//                throw new InvalidOperationException(
//                    $"curl_url_get returned error: {result}"
//                );
//            }
//
//            var iptr = (IntPtr)freeMe;
//            return iptr;
//        }
//
//        public static unsafe void UpdateUriPart(IntPtr curluHandle, CURLUPart part, string value, ulong flags)
//        {
//            var result = libcurl.curl_url_set((void*)curluHandle, part, value, flags);
//            if (result != CURLUcode.CURLUE_OK) {
//                throw new InvalidOperationException(
//                    $"curl_url_set returned error: {result}"
//                );
//            }
//        }
//        
//        public static CURI TryParse(string input)
//        {
//            
//            return default;
//        }
//
//        public static unsafe IntPtr Alloc()
//        {
//            var handle = libcurl.curl_url();
//            if (handle == null)
//                throw new InvalidOperationException("curl_url returned NULL.");
//            return (IntPtr)handle;
//        }
//
//    }
//    
//    public unsafe class CURI : CurlMemory
//    {
//        private string? _url;
//
//        public string Url
//        {
//            get {
//                if (!string.IsNullOrWhiteSpace(_url))
//                    return _url;
//
//                var ptr = CURIModule.UriPart(Handle, CURLUPart.URL, CURLUflags.CURLU_DEFAULT_PORT);
//                _freeze.Add(ptr);
//                _url = Marshal.PtrToStringAnsi(ptr);
//                return _url;
//            }
//        }
//        
//        public string Scheme { get; }
//        public string User { get; }
//        public string Password { get; }
//        public string Options { get; }
//        public string Host { get; }
//        public int Port { get; }
//
//        private readonly List<IntPtr> _freeze;
//        
//        protected override unsafe void Delete()
//        {
//            foreach (var ptr in _freeze)
//                libc.free((void*)ptr);
//            libcurl.curl_curl_cleanup((void*)Handle);
//        }
//        
//        internal CURI(in IntPtr handle) : base(in handle, NullPtrPolicy.NotAllowed)
//        {
//            _freeze = new List<IntPtr>();
//        }
//
//        public CURI(string url) : this(CURIModule.Alloc())
//        {
//            CURIModule.UpdateUriPart(Handle, CURLUPart.URL, url, 0);
//        }
//        
//
//    }
}
