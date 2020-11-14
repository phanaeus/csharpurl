using System;
using System.Runtime.InteropServices;

namespace CSharpUrl.LibC
{
    internal static class libc
    {
#if WINDOWS
        private const string LibC = "ucrtbase";
#else
        private const string LibC = "libc";
#endif

        [DllImport(LibC, CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern void* calloc(int nmemb, int size);
        [DllImport(LibC, CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern void free(void* ptr);
    }

    public static unsafe class LibCModule
    {
        public static IntPtr Calloc(int nmemb, int size)
        {
            var ptr = libc.calloc(nmemb, size);
            if (ptr == null)
                throw new OutOfMemoryException("calloc returned NULL.");
            return new IntPtr(ptr);
        }

        public static IntPtr Calloc(int size)
        {
            return Calloc(1, size);
        }

        public static void Free(in IntPtr ptr) => libc.free((void*)ptr);
    }
}
