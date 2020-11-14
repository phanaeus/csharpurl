﻿using System;

namespace CSharpUrl.LibUv
{
    public enum uv_handle_type
    {
        UV_UNKNOWN_HANDLE = 0,
        UV_ASYNC,
        UV_CHECK,
        UV_FS_EVENT,
        UV_FS_POLL,
        UV_HANDLE,
        UV_IDLE,
        UV_NAMED_PIPE,
        UV_POLL,
        UV_PREPARE,
        UV_PROCESS,
        UV_STREAM,
        UV_TCP,
        UV_TIMER,
        UV_TTY,
        UV_UDP,
        UV_SIGNAL,
        UV_FILE,
        UV_HANDLE_TYPE_MAX
    }

    public enum uv_err_code
    {
        UV_EOF = -4095,
        UV_OK = 0,
        UV_E2BIG,
        UV_EACCES,
        UV_EADDRINUSE,
        UV_EADDRNOTAVAIL,
        UV_EAFNOSUPPORT,
        UV_EAGAIN,
        UV_EAI_ADDRFAMILY,
        UV_EAI_AGAIN,
        UV_EAI_BADFLAGS,
        UV_EAI_BADHINTS,
        UV_EAI_CANCELED,
        UV_EAI_FAIL,
        UV_EAI_FAMILY,
        UV_EAI_MEMORY,
        UV_EAI_NODATA,
        UV_EAI_NONAME,
        UV_EAI_OVERFLOW,
        UV_EAI_PROTOCOL,
        UV_EAI_SERVICE,
        UV_EAI_SOCKTYPE,
        UV_EALREADY,
        UV_EBADF,
        UV_EBUSY,
        UV_ECANCELED,
        UV_ECHARSET,
        UV_ECONNABORTED,
        UV_ECONNREFUSED,
        UV_ECONNRESET,
        UV_EDESTADDRREQ,
        UV_EEXIST,
        UV_EFAULT,
        UV_EFBIG,
        UV_EHOSTUNREACH,
        UV_EINTR,
        UV_EINVAL,
        UV_EIO,
        UV_EISCONN,
        UV_EISDIR,
        UV_ELOOP,
        UV_EMFILE,
        UV_EMSGSIZE,
        UV_ENAMETOOLONG,
        UV_ENETDOWN,
        UV_ENETUNREACH,
        UV_ENFILE,
        UV_ENOBUFS,
        UV_ENODEV,
        UV_ENOENT,
        UV_ENOMEM,
        UV_ENONET,
        UV_ENOPROTOOPT,
        UV_ENOSPC,
        UV_ENOSYS,
        UV_ENOTCONN,
        UV_ENOTDIR,
        UV_ENOTEMPTY,
        UV_ENOTSOCK,
        UV_ENOTSUP,
        UV_EPERM,
        UV_EPIPE,
        UV_EPROTO,
        UV_EPROTONOSUPPORT,
        UV_EPROTOTYPE,
        UV_ERANGE,
        UV_EROFS,
        UV_ESHUTDOWN,
        UV_ESPIPE,
        UV_ESRCH,
        UV_ETIMEDOUT,
        UV_ETXTBSY,
        UV_EXDEV,
        UV_UNKNOWN,
        UV_ENXIO,
        UV_EMLINK,
    }

    public enum uv_run_mode
    {
        UV_RUN_DEFAULT = 0,
        UV_RUN_ONCE,
        UV_RUN_NOWAIT
    }

    public enum uv_req_type
    {
        UV_UNKNOWN_REQ = 0,
        UV_REQ,
        UV_CONNECT,
        UV_WRITE,
        UV_SHUTDOWN,
        UV_UDP_SEND,
        UV_FS,
        UV_WORK,
        UV_GETADDRINFO,
        UV_GETNAMEINFO,
        UV_REQ_TYPE_PRIVATE,
        UV_REQ_TYPE_MAX
    }

    [Flags]
    public enum uv_poll_event
    {
        NONE,
        UV_READABLE = 1,
        UV_WRITABLE = 2,
        UV_DISCONNECT = 4,
        UV_PRIORITIZED = 8
    }
}
