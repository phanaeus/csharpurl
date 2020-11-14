using System;

namespace CSharpUrl.LibUv
{
#pragma warning disable RCS1194 // Implement exception constructors.
    public class UvException : InvalidOperationException
#pragma warning restore RCS1194 // Implement exception constructors.
    {
        public uv_err_code Code { get; }
        public string UvErrMsg { get; }

        public UvException(string message, uv_err_code code) : base(message)
        {
            Code = code;
            UvErrMsg = UvModule.UvStrError(code);
        }
    }
}
