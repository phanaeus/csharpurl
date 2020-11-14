using System;
using System.Runtime.InteropServices;

namespace CSharpUrl.LibUv
{
#pragma warning disable IDE1006 // Naming Styles
    [StructLayout(LayoutKind.Sequential)]
    public struct uv_handle_t
    {
        public IntPtr data;
        public IntPtr loop;
        public uv_handle_type type;
        public IntPtr close_cb;
    }

    public struct uv_buf_t
    {
        public unsafe byte* @base;
        public ulong len;
    }
#pragma warning restore IDE1006 // Naming Styles
}
