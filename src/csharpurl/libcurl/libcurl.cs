using System;
using System.Runtime.InteropServices;

namespace CSharpUrl.LibCurl
{
    public static class libcurl
    {
        private const string LibCurl = "libcurl";

//#if DOWS
//        [DllImport("kernel32", SetLastError = true)]
//        private static extern IntPtr LoadLibraryW([MarshalAs(UnmanagedType.LPWStr)]string lpFileName);

//        static libcurl()
//        {
//            var ptr = LoadLibraryW("libcurl.dll");
//            if (ptr == IntPtr.Zero)
//                Environment.FailFast("Failed to load libcurl.dll. Is it located in application directory?");
//            ptr = LoadLibraryW("libuv.dll");
//            if (ptr == IntPtr.Zero)
//                Environment.FailFast("Failed to load libuv.dll. Is it located in application directory?");
//        }
//#endif
//#if NIX
//        [DllImport("libdl")]
//        private static extern IntPtr dlopen([MarshalAs(UnmanagedType.LPStr)]string fileName, int flags);

//        static libcurl()
//        {
//            var ptr = dlopen("/usr/lib/x86_64-linux-gnu/libcurl.so", 2);
//            if (ptr == IntPtr.Zero)
//                Environment.FailFast("Failed to load libcurl.so. Is it located in application directory?");
//            ptr = dlopen("libuv.so", 2);
//            if (ptr == IntPtr.Zero)
//                Environment.FailFast("Failed to load libuv.so. Is it located in application directory?");
//        }
//#endif


        [DllImport(LibCurl, CallingConvention = CallingConvention.Cdecl)]
        public static extern CURLcode curl_global_init(CURLglobal flags = CURLglobal.DEFAULT);

        [DllImport(LibCurl, CallingConvention = CallingConvention.Cdecl)]
        public static extern void curl_global_cleanup();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int write_callback(IntPtr data, int size, int nmemb, IntPtr userdata);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public unsafe delegate int unsafe_write_callback(byte* data, int size, int nmemb, void* userdata);

        [DllImport(LibCurl, EntryPoint = "curl_easy_init", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr _curl_easy_init();

        [DllImport(LibCurl, EntryPoint = "curl_version", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe IntPtr _curl_version();

        public static string curl_version()
        {
            var ptr = _curl_version();
            return Marshal.PtrToStringAnsi(ptr);
        }

        public static CurlEzHandle curl_easy_init()
        {
            var handle = _curl_easy_init();
            if (handle == IntPtr.Zero)
                throw new OutOfMemoryException("uh oh. curl_easy_init returned NULL.");
            return new CurlEzHandle(handle);
        }

        [DllImport(LibCurl, CallingConvention = CallingConvention.Cdecl)]
        public static extern void curl_easy_cleanup(IntPtr handle);

        [DllImport(LibCurl, CallingConvention = CallingConvention.Cdecl)]
        public static extern CURLcode curl_easy_perform(IntPtr handle);

        [DllImport(LibCurl, CallingConvention = CallingConvention.Cdecl)]
        public static extern void curl_easy_reset(IntPtr handle);

        [DllImport(LibCurl, CallingConvention = CallingConvention.Cdecl)]
        public static extern CURLcode curl_easy_setopt(IntPtr handle, CURLoption option, int value);

        [DllImport(LibCurl, CallingConvention = CallingConvention.Cdecl)]
        public static extern CURLcode curl_easy_setopt(IntPtr handle, CURLoption option, IntPtr value);

        [DllImport(LibCurl, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern CURLcode curl_easy_setopt(IntPtr handle, CURLoption option, [MarshalAs(UnmanagedType.LPStr)]string value);

        [DllImport(LibCurl, CallingConvention = CallingConvention.Cdecl)]
        public static extern CURLcode curl_easy_setopt(IntPtr handle, CURLoption option, byte[] value);

        [DllImport(LibCurl, CallingConvention = CallingConvention.Cdecl)]
        public static extern CURLcode curl_easy_setopt(IntPtr handle, CURLoption option, write_callback value);

        [DllImport(LibCurl, CallingConvention = CallingConvention.Cdecl)]
        public static extern CURLcode curl_easy_setopt(IntPtr handle, CURLoption option, unsafe_write_callback value);

        [DllImport(LibCurl, CallingConvention = CallingConvention.Cdecl)]
        public static extern CURLcode curl_easy_getinfo(IntPtr handle, CURLINFO option, out int value);

        [DllImport(LibCurl, CallingConvention = CallingConvention.Cdecl)]
        public static extern CURLcode curl_easy_getinfo(IntPtr handle, CURLINFO option, out IntPtr value);

        [DllImport(LibCurl, CallingConvention = CallingConvention.Cdecl)]
        public static extern CURLcode curl_easy_getinfo(IntPtr handle, CURLINFO option, out double value);

        [DllImport(LibCurl, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern CURLcode curl_easy_getinfo(IntPtr handle, CURLINFO option, IntPtr value);

        [DllImport(LibCurl, EntryPoint = "curl_easy_strerror", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern IntPtr _curl_easy_strerror(CURLcode errornum);

        public static unsafe string curl_easy_strerror(CURLcode code)
        {
            var ptr = _curl_easy_strerror(code);
            return Marshal.PtrToStringAnsi(ptr);
        }


        [DllImport(LibCurl, EntryPoint = "curl_multi_init", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr _curl_multi_init();

        public static CurlMultiHandle curl_multi_init()
        {
            var handle = _curl_multi_init();
            if (handle == IntPtr.Zero)
                throw new OutOfMemoryException("uh oh. curl_multi_init returned NULL.");
            return new CurlMultiHandle(handle);
        }

        [DllImport(LibCurl, CallingConvention = CallingConvention.Cdecl)]
        public static extern CURLMcode curl_multi_cleanup(IntPtr multiHandle);

        [DllImport(LibCurl, CallingConvention = CallingConvention.Cdecl)]
        public static extern CURLMcode curl_multi_add_handle(IntPtr multiHandle, IntPtr easyHandle);

        [DllImport(LibCurl, CallingConvention = CallingConvention.Cdecl)]
        public static extern CURLMcode curl_multi_remove_handle(IntPtr multiHandle, IntPtr easyHandle);

        [DllImport(LibCurl, CallingConvention = CallingConvention.Cdecl)]
        public static extern CURLMcode curl_multi_setopt(IntPtr multiHandle, CURLMoption option, int value);


        [DllImport(LibCurl, CallingConvention = CallingConvention.Cdecl)]
        public static extern CURLMcode curl_multi_setopt(IntPtr multiHandle, CURLMoption option, IntPtr value);


        [DllImport(LibCurl, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr curl_multi_info_read(IntPtr multiHandle, out int msgsInQueue);

        [DllImport(LibCurl, CallingConvention = CallingConvention.Cdecl)]
        public static extern CURLMcode curl_multi_socket_action(
            IntPtr multiHandle,
            int sockfd,
            CURLcselect evBitmask,
            out int runningHandles);

        [DllImport(LibCurl, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr curl_multi_strerror(CURLMcode errornum);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int timer_callback(IntPtr multiHandle, int timeoutMs, IntPtr userp);

        [DllImport(LibCurl, CallingConvention = CallingConvention.Cdecl)]
        public static extern CURLMcode curl_multi_setopt(IntPtr multiHandle, CURLMoption option, timer_callback value);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int socket_callback(IntPtr easy, int s, CURLpoll what, IntPtr userp, IntPtr socketp);

        [DllImport(LibCurl, CallingConvention = CallingConvention.Cdecl)]
        public static extern CURLMcode curl_multi_setopt(IntPtr multiHandle, CURLMoption option,
            socket_callback value);

        [DllImport(LibCurl, EntryPoint = "curl_slist_append", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr _curl_slist_append(IntPtr slist, [MarshalAs(UnmanagedType.LPStr)]string data);

        public static void curl_slist_append(CurlSlist slist, string data)
        {
            var handle = _curl_slist_append(slist, data);
            if (handle == IntPtr.Zero)
                throw new OutOfMemoryException("curl_slist_append returned NULL.");
            slist.SetHandle(handle);
        }

        [DllImport(LibCurl, CallingConvention = CallingConvention.Cdecl)]
        public static extern void curl_slist_free_all(IntPtr pList);

        [DllImport(LibCurl, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void* curl_url();

        [DllImport(LibCurl, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void curl_curl_cleanup(void* handle);

        [DllImport(LibCurl, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe CURLUcode curl_url_get(
            void* curluHandle,
            CURLUPart part,
            byte** freeMe,
            ulong flags
        );

        [DllImport(LibCurl, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe CURLUcode curl_url_set(
            void* curluHandle, 
            CURLUPart part, 
            [MarshalAs(UnmanagedType.LPStr)]string content,
            ulong flags);
    }
}
