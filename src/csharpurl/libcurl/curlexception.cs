using System;
using CSharpUrl.LibCurl;

namespace CSharpUrl
{
    public class CurlException : InvalidOperationException
    {
        public CurlException()
        {
        }

        public CurlException(string message) : base(message)
        {
        }

        public CurlException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

    public class CurlECodeException : CurlException
    {
        public CurlECodeException(string message, CURLcode code) : base(message)
        {
            var errMsg = libcurl.curl_easy_strerror(code);
            Code = code;
            CurlErrMsg = errMsg;
        }

        public CURLcode Code { get; }
        public string CurlErrMsg { get; }
    }

    public class CurlMultiECodeException : CurlException
    {
        public CurlMultiECodeException(string message, CURLMcode code) : base(message)
        {
            var errMsg = Curl.CurlMultiStrErr(code);
            Code = code;
            CurlErrMsg = errMsg;
        }

        public CURLMcode Code { get; }
        public string CurlErrMsg { get; }
    }
}
