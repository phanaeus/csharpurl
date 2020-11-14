using System.Runtime.InteropServices;

namespace CSharpUrl.LibUv
{
    public static class UvModule
    {
        public static void ValidateResult(uv_err_code result)
        {
            if (result == uv_err_code.UV_OK) return;
            throw new UvException($"libuv returned error: {result}", result);
        }

        public static string UvStrError(uv_err_code code) => Marshal.PtrToStringAnsi(libuv.uv_strerror(code));
    }
}
