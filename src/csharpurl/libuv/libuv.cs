using System;
using System.Runtime.InteropServices;

namespace CSharpUrl.LibUv
{
    public static class libuv
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void uv_poll_cb(IntPtr handle, int status, int events);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void uv_timer_cb(IntPtr handle);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void uv_walk_cb(IntPtr handle, IntPtr arg);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void uv_alloc_cb(IntPtr handle, int suggestedSize, IntPtr buf);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void uv_write_cb(IntPtr req, int status);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int uv_fileno_func(IntPtr handle, ref IntPtr socket);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void uv_close_cb(IntPtr handle);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void uv_async_cb(IntPtr handle);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void uv_connection_cb(IntPtr server, int status);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void uv_connect_cb(IntPtr req, int status);

        //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        //public delegate void uv_read_cb(IntPtr server, int nread, ref uv_buf_t buf);

        //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        //public delegate void uv_alloc_cb(IntPtr server, int suggested_size, out uv_buf_t buf);

        [DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
        public static extern uv_err_code uv_loop_init(IntPtr handle);

        [DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
        public static extern uv_err_code uv_loop_close(IntPtr a0);

        [DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
        public static extern uv_err_code uv_run(IntPtr handle, uv_run_mode mode);

        [DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
        public static extern void uv_stop(IntPtr handle);

        [DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
        public static extern void uv_ref(IntPtr handle);

        [DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
        public static extern void uv_unref(IntPtr handle);

        [DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
        public static extern int uv_fileno(IntPtr handle, ref IntPtr socket);

        [DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
        public static extern void uv_close(IntPtr handle, uv_close_cb close_cb);

        [DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
        public static extern uv_err_code uv_async_init(IntPtr loop, IntPtr handle, uv_async_cb cb);

        [DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
        public extern static uv_err_code uv_async_send(IntPtr handle);

        [DllImport("libuv", CallingConvention = CallingConvention.Cdecl, EntryPoint = "uv_async_send")]
        public extern static int uv_unsafe_async_send(IntPtr handle);

        [DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
        public static extern uv_err_code uv_tcp_init(IntPtr loop, IntPtr handle);

        //[DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
        //public static extern int uv_tcp_bind(IntPtr handle, ref sockaddr addr, int flags);

        [DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
        public static extern int uv_tcp_open(IntPtr handle, IntPtr hSocket);

        [DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
        public static extern uv_err_code uv_tcp_nodelay(IntPtr handle, int enable);

        [DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
        public static extern uv_err_code uv_tcp_keepalive(IntPtr handle, int enable, uint delay);

        [DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
        public static extern int uv_pipe_init(IntPtr loop, IntPtr handle, int ipc);

        [DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
        public static extern int uv_pipe_bind(IntPtr loop, string name);

        [DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
        public static extern int uv_pipe_open(IntPtr handle, IntPtr hSocket);

        [DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
        public static extern int uv_listen(IntPtr handle, int backlog, uv_connection_cb cb);

        [DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
        public static extern int uv_accept(IntPtr server, IntPtr client);

        [DllImport("libuv", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void uv_pipe_connect(IntPtr req, IntPtr handle, string name, uv_connect_cb cb);

        [DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
        public extern static int uv_pipe_pending_count(IntPtr handle);

        //[DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
        //public extern static int uv_read_start(IntPtr handle, uv_alloc_cb alloc_cb, uv_read_cb read_cb);

        [DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
        public static extern int uv_read_stop(IntPtr handle);

        //[DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
        //public static extern int uv_try_write(IntPtr handle, uv_buf_t[] bufs, int nbufs);

        //[DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
        //public static unsafe extern int uv_write(IntPtr req, IntPtr handle, uv_buf_t* bufs, int nbufs, uv_write_cb cb);

        //[DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
        //public static unsafe extern int uv_write2(IntPtr req, IntPtr handle, uv_buf_t* bufs, int nbufs, IntPtr sendHandle, uv_write_cb cb);

        [DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
        public extern static IntPtr uv_err_name(int err);

        [DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr uv_strerror(uv_err_code err);

        [DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
        public static extern int uv_loop_size();

        [DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
        public static extern int uv_handle_size(uv_handle_type handleType);

        [DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
        public static extern int uv_req_size(uv_req_type reqType);

        //[DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
        //public static extern int uv_ip4_addr(string ip, int port, out sockaddr addr);

        //[DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
        //public static extern int uv_ip6_addr(string ip, int port, out sockaddr addr);

        //[DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
        //public static extern int uv_tcp_getsockname(IntPtr handle, out sockaddr name, ref int namelen);

        //[DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
        //public static extern int uv_tcp_getpeername(IntPtr handle, out sockaddr name, ref int namelen);

        [DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
        public static extern int uv_walk(IntPtr loop, uv_walk_cb walk_cb, IntPtr arg);

        [DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
        public static extern uv_err_code uv_timer_init(IntPtr loop, IntPtr handle);

        [DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
        public static extern uv_err_code uv_timer_start(IntPtr handle, uv_timer_cb cb, long timeout, long repeat);

        [DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
        public static extern uv_err_code uv_timer_stop(IntPtr handle);

        [DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
        public static extern long uv_now(IntPtr loop);

        [DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
        public static extern uv_err_code uv_poll_init(IntPtr loop, IntPtr handle, IntPtr fd);

        [DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
        public static extern uv_err_code uv_poll_init_socket(IntPtr loop, IntPtr handle, int socket);

        [DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
        public static extern uv_err_code uv_poll_start(IntPtr handle, int events, uv_poll_cb cb);

        [DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
        public static extern uv_err_code uv_poll_stop(IntPtr handle);
    }}
