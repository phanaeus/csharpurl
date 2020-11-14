using System;
using System.Runtime.InteropServices;

namespace CSharpUrl.LibCurl
{
    [StructLayout(LayoutKind.Explicit)]
    public struct CURLMsgData
    {
        [FieldOffset(0)] public IntPtr whatever; /* (void*) message-specific data */
        [FieldOffset(0)] public CURLcode result; /* return code for transfer */
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct CURLMsg
    {
        public CURLMSG msg; /* what this message means */
        public IntPtr easy_handle; /* the handle it concerns */
        public CURLMsgData data;
    }
}
