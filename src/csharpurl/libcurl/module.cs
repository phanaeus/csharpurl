using System;
using System.Runtime.InteropServices;

namespace CSharpUrl.LibCurl
{
    public static class Curl
    {
        public static string CurlMultiStrErr(CURLMcode code)
        {
            var ptr = libcurl.curl_multi_strerror(code);
            return Marshal.PtrToStringAnsi(ptr);
        }

        public static void ValidateSetOptResult(CURLcode code)
        {
            if (code == CURLcode.OK)
                return;
            throw new CurlECodeException("curl_easy_setopt returned error", code);
        }

        public static void ValidateGetInfoResult(CURLcode code)
        {
            if (code == CURLcode.OK)
                return;
            throw new CurlECodeException("curl_easy_getinfo returned error", code);
        }

        public static void ValidateMultiResult(CURLMcode code)
        {
            if (code == CURLMcode.OK)
                return;
            throw new CurlMultiECodeException($"curl_multi function returned error: {code}", code);
        }

        public static int CurlMultiSocketAction(CurlMultiHandle multi, int sockfd, CURLcselect select)
        {
            ValidateMultiResult(
                libcurl.curl_multi_socket_action(multi, sockfd, select, out var result)
            );
            return result;
        }

        public static void MultiAddEz(CurlMultiHandle multi, CurlEzHandle ez)
        {
            ValidateMultiResult(
                libcurl.curl_multi_add_handle(multi, ez)
            );
        }

        public static void MultiSetOpt(CurlMultiHandle multi, CURLMoption option, libcurl.socket_callback value)
        {
            ValidateMultiResult(
                libcurl.curl_multi_setopt(multi, option, value)
            );
        }

        public static void MultiSetOpt(CurlMultiHandle multi, CURLMoption option, int value)
        {
            ValidateMultiResult(
                libcurl.curl_multi_setopt(multi, option, value)
            );
        }

        public static void MultiSetOpt(CurlMultiHandle multi, CURLMoption option, libcurl.timer_callback value)
        {
            ValidateMultiResult(
                libcurl.curl_multi_setopt(multi, option, value)
            );
        }

        public static void MultiSetOpt(CurlMultiHandle multi, CURLMoption option, IntPtr value)
        {
            ValidateMultiResult(
                libcurl.curl_multi_setopt(multi, option, value)
            );
        }

        public static void MultiRemoveEz(CurlMultiHandle multi, IntPtr ez)
        {
            ValidateMultiResult(
                libcurl.curl_multi_remove_handle(multi, ez)
            );
        }
    }
}
