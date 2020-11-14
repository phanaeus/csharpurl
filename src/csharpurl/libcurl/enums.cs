
using System;

// most of these enum definitions copied from https://github.com/stil/CurlThin/tree/master/CurlThin/Enums

namespace CSharpUrl.LibCurl
{
    /// <summary>
    ///     Reference: https://github.com/curl/curl/blob/master/include/curl/curl.h
    /// </summary>
    public enum CURLcode : uint
    {
        OK = 0,

        /// <summary>1</summary>
        UNSUPPORTED_PROTOCOL,

        /// <summary>2</summary>
        FAILED_INIT,

        /// <summary>3</summary>
        URL_MALFORMAT,

        /// <summary>4 - [was obsoleted in August 2007 for 7.17.0, reused in April 2011 for 7.21.5]</summary>
        NOT_BUILT_IN,

        /// <summary>5</summary>
        COULDNT_RESOLVE_PROXY,

        /// <summary>6</summary>
        COULDNT_RESOLVE_HOST,

        /// <summary>7</summary>
        COULDNT_CONNECT,

        /// <summary>8</summary>
        WEIRD_SERVER_REPLY,

        /// <summary>9 a service was denied by the server due to lack of access - when login fails this is not returned.</summary>
        REMOTE_ACCESS_DENIED,

        /// <summary>10 - [was obsoleted in April 2006 for 7.15.5, reused in Dec 2011 for 7.24.0]</summary>
        FTP_ACCEPT_FAILED,

        /// <summary>11</summary>
        FTP_WEIRD_PASS_REPLY,

        /// <summary>
        ///     12 - timeout occurred accepting server
        ///     [was obsoleted in August 2007 for 7.17.0, reused in Dec 2011 for 7.24.0]
        /// </summary>
        FTP_ACCEPT_TIMEOUT,

        /// <summary>13</summary>
        FTP_WEIRD_PASV_REPLY,

        /// <summary>14</summary>
        FTP_WEIRD_227_FORMAT,

        /// <summary>15</summary>
        FTP_CANT_GET_HOST,

        /// <summary>
        ///     16 - A problem in the http2 framing layer.
        ///     [was obsoleted in August 2007 for 7.17.0, reused in July 2014 for 7.38.0]
        /// </summary>
        HTTP2,

        /// <summary>17</summary>
        FTP_COULDNT_SET_TYPE,

        /// <summary>18</summary>
        PARTIAL_FILE,

        /// <summary>19</summary>
        FTP_COULDNT_RETR_FILE,

        /// <summary>20 - NOT USED</summary>
        OBSOLETE20,

        /// <summary>21 - quote command failure</summary>
        QUOTE_ERROR,

        /// <summary>22</summary>
        HTTP_RETURNED_ERROR,

        /// <summary>23</summary>
        WRITE_ERROR,

        /// <summary>24 - NOT USED</summary>
        OBSOLETE24,

        /// <summary>25 - failed upload "command"</summary>
        UPLOAD_FAILED,

        /// <summary>26 - couldn't open/read from file</summary>
        READ_ERROR,

        /// <summary>
        ///     27 - Note: OUT_OF_MEMORY may sometimes indicate a conversion error instead of a memory allocation error if
        ///     CURL_DOES_CONVERSIONS is defined
        /// </summary>
        OUT_OF_MEMORY,

        /// <summary>28 - the timeout time was reached</summary>
        OPERATION_TIMEDOUT,

        /// <summary>29 - NOT USED</summary>
        OBSOLETE29,

        /// <summary>30 - FTP PORT operation failed</summary>
        FTP_PORT_FAILED,

        /// <summary>31 - the REST command failed</summary>
        FTP_COULDNT_USE_REST,

        /// <summary>32 - NOT USED</summary>
        OBSOLETE32,

        /// <summary>33 - RANGE "command" didn't work</summary>
        RANGE_ERROR,

        /// <summary>34</summary>
        HTTP_POST_ERROR,

        /// <summary>35 - wrong when connecting with SSL</summary>
        SSL_CONNECT_ERROR,

        /// <summary>36 - couldn't resume download</summary>
        BAD_DOWNLOAD_RESUME,

        /// <summary>37</summary>
        FILE_COULDNT_READ_FILE,

        /// <summary>38</summary>
        LDAP_CANNOT_BIND,

        /// <summary>39</summary>
        LDAP_SEARCH_FAILED,

        /// <summary>40 - NOT USED</summary>
        OBSOLETE40,

        /// <summary>41 - NOT USED starting with 7.53.0</summary>
        FUNCTION_NOT_FOUND,

        /// <summary>42</summary>
        ABORTED_BY_CALLBACK,

        /// <summary>43</summary>
        BAD_FUNCTION_ARGUMENT,

        /// <summary>44 - NOT USED</summary>
        OBSOLETE44,

        /// <summary>45 - CURLOPT_INTERFACE failed</summary>
        INTERFACE_FAILED,

        /// <summary>46 - NOT USED</summary>
        OBSOLETE46,

        /// <summary>47 - catch endless re-direct loops</summary>
        TOO_MANY_REDIRECTS,

        /// <summary>48 - User specified an unknown option</summary>
        UNKNOWN_OPTION,

        /// <summary>49 - Malformed telnet option</summary>
        TELNET_OPTION_SYNTAX,

        /// <summary>50 - NOT USED</summary>
        OBSOLETE50,

        /// <summary>51 - peer's certificate or fingerprint wasn't verified fine</summary>
        PEER_FAILED_VERIFICATION,

        /// <summary>52 - when this is a specific error</summary>
        GOT_NOTHING,

        /// <summary>53 - SSL crypto engine not found</summary>
        SSL_ENGINE_NOTFOUND,

        /// <summary>54 - can not set SSL crypto engine as default</summary>
        SSL_ENGINE_SETFAILED,

        /// <summary>55 - failed sending network data</summary>
        SEND_ERROR,

        /// <summary>56 - failure in receiving network data</summary>
        RECV_ERROR,

        /// <summary>57 - NOT IN USE</summary>
        OBSOLETE57,

        /// <summary>58 - problem with the local certificate</summary>
        SSL_CERTPROBLEM,

        /// <summary>59 - couldn't use specified cipher</summary>
        SSL_CIPHER,

        /// <summary>60 - problem with the CA cert (path?)</summary>
        SSL_CACERT,

        /// <summary>61 - Unrecognized/bad encoding</summary>
        BAD_CONTENT_ENCODING,

        /// <summary>62 - Invalid LDAP URL</summary>
        LDAP_INVALID_URL,

        /// <summary>63 - Maximum file size exceeded</summary>
        FILESIZE_EXCEEDED,

        /// <summary>64 - Requested FTP SSL level failed</summary>
        USE_SSL_FAILED,

        /// <summary>65 - Sending the data requires a rewind that failed</summary>
        SEND_FAIL_REWIND,

        /// <summary>66 - failed to initialise ENGINE</summary>
        SSL_ENGINE_INITFAILED,

        /// <summary>67 - user, password or similar was not accepted and we failed to login</summary>
        LOGIN_DENIED,

        /// <summary>68 - file not found on server</summary>
        TFTP_NOTFOUND,

        /// <summary>69 - permission problem on server</summary>
        TFTP_PERM,

        /// <summary>70 - out of disk space on server</summary>
        REMOTE_DISK_FULL,

        /// <summary>71 - Illegal TFTP operation</summary>
        TFTP_ILLEGAL,

        /// <summary>72 - Unknown transfer ID</summary>
        TFTP_UNKNOWNID,

        /// <summary>73 - File already exists</summary>
        REMOTE_FILE_EXISTS,

        /// <summary>74 - No such user</summary>
        TFTP_NOSUCHUSER,

        /// <summary>75 - conversion failed</summary>
        CONV_FAILED,

        /// <summary>
        ///     76 - caller must register conversion callbacks using curl_easy_setopt options
        ///     CURLOPT_CONV_FROM_NETWORK_FUNCTION,
        ///     CURLOPT_CONV_TO_NETWORK_FUNCTION, and
        ///     CURLOPT_CONV_FROM_UTF8_FUNCTION
        /// </summary>
        CONV_REQD,

        /// <summary>77 - could not load CACERT file, missing or wrong format</summary>
        SSL_CACERT_BADFILE,

        /// <summary>78 - remote file not found</summary>
        REMOTE_FILE_NOT_FOUND,

        /// <summary>
        ///     79 - error from the SSH layer, somewhat generic so the error message will be of interest when this has
        ///     happened
        /// </summary>
        SSH,

        /// <summary>80 - Failed to shut down the SSL connection</summary>
        SSL_SHUTDOWN_FAILED,

        /// <summary>81 - socket is not ready for send/recv, wait till it's ready and try again (Added in 7.18.2)</summary>
        AGAIN,

        /// <summary>82 - could not load CRL file, missing or wrong format (Added in 7.19.0)</summary>
        SSL_CRL_BADFILE,

        /// <summary>83 - Issuer check failed. (Added in 7.19.0)</summary>
        SSL_ISSUER_ERROR,

        /// <summary>84 - a PRET command failed</summary>
        FTP_PRET_FAILED,

        /// <summary>85 - mismatch of RTSP CSeq numbers</summary>
        RTSP_CSEQ_ERROR,

        /// <summary> 86 - mismatch of RTSP Session Ids</summary>
        RTSP_SESSION_ERROR,

        /// <summary>87 - unable to parse FTP file list</summary>
        FTP_BAD_FILE_LIST,

        /// <summary>88 - chunk callback reported error</summary>
        CHUNK_FAILED,

        /// <summary>89 - No connection available, the session will be queued</summary>
        NO_CONNECTION_AVAILABLE,

        /// <summary>90 - specified pinned public key did not match</summary>
        SSL_PINNEDPUBKEYNOTMATCH,

        /// <summary>91 - invalid certificate status</summary>
        SSL_INVALIDCERTSTATUS,

        /// <summary>92 - stream error in HTTP/2 framing layer</summary>
        HTTP2_STREAM,

        /// <summary>never use!</summary>
        CURL_LAST
    }

    [Flags]
    public enum CURLcselect
    {
        NONE = 0,
        IN = 0x01,
        OUT = 0x02,
        ERR = 0x04
    }

    public enum CURL_SSLVERSION
    {
        DEFAULT,
        TLSv1, /* TLS 1.x */
        SSLv2,
        SSLv3,
        TLSv1_0,
        TLSv1_1,
        TLSv1_2,
        TLSv1_3,
        LAST /* never use, keep last */
    };

    public enum CURL_SSLVERSION_MAX
    {
        NONE = 0,
        DEFAULT = (CURL_SSLVERSION.TLSv1 << 16),
        TLSv1_0 = (CURL_SSLVERSION.TLSv1_0 << 16),
        TLSv1_1 = (CURL_SSLVERSION.TLSv1_1 << 16),
        TLSv1_2 = (CURL_SSLVERSION.TLSv1_2 << 16),
        TLSv1_3 = (CURL_SSLVERSION.TLSv1_3 << 16),
        /* never use, keep last */
        LAST = (CURL_SSLVERSION.LAST << 16)
    }

    [Flags]
    public enum CURLglobal
    {
        /// <summary>
        ///     Initialise nothing extra. This sets no bit.
        /// </summary>
        NOTHING = 0,

        /// <summary>
        ///     Initialize SSL.
        ///     The implication here is that if this bit is not set, the initialization of the SSL layer needs to be done by the
        ///     application or at least outside of libcurl. The exact procedure how to do SSL initializtion depends on the
        ///     TLS backend libcurl uses.
        ///     Doing TLS based transfers without having the TLS layer initialized may lead to unexpected behaviors.
        /// </summary>
        SSL = 1 << 0,

        /// <summary>
        ///     Initialize the Win32 socket libraries.
        ///     The implication here is that if this bit is not set, the initialization of winsock has to be done by the
        ///     application or you risk getting undefined behaviors. This option exists for when the initialization is
        ///     handled outside of libcurl so there's no need for libcurl to do it again.
        /// </summary>
        WIN32 = 1 << 1,

        /// <summary>
        ///     When this flag is set, curl will acknowledge EINTR condition when connecting or when waiting for data.
        ///     Otherwise, curl waits until full timeout elapses. (Added in 7.30.0)
        /// </summary>
        ACK_EINTR = 1 << 2,

        /// <summary>
        ///     Initialize everything possible. This sets all known bits except <see cref="ACK_EINTR" />.
        /// </summary>
        ALL = SSL | WIN32,

        /// <summary>
        ///     A sensible default. It will init both SSL and Win32. Right now, this equals the functionality of the
        ///     <see cref="ALL" /> mask.
        /// </summary>
        DEFAULT = ALL
    }

    internal static class CURLINFOTYPE
    {
        public const uint STRING = 0x100000;
        public const uint LONG = 0x200000;
        public const uint DOUBLE = 0x300000;
        public const uint SLIST = 0x400000;
        public const uint PTR = 0x400000; // same as SLIST
        public const uint SOCKET = 0x500000;
        public const uint OFF_T = 0x600000;
        public const uint MASK = 0x0fffff;
        public const uint TYPEMASK = 0xf00000;
    }

    public enum CURLINFO : uint
    {
        NONE, // First, never use this.
        EFFECTIVE_URL = CURLINFOTYPE.STRING + 1,
        RESPONSE_CODE = CURLINFOTYPE.LONG + 2,
        TOTAL_TIME = CURLINFOTYPE.DOUBLE + 3,
        NAMELOOKUP_TIME = CURLINFOTYPE.DOUBLE + 4,
        CONNECT_TIME = CURLINFOTYPE.DOUBLE + 5,
        PRETRANSFER_TIME = CURLINFOTYPE.DOUBLE + 6,
        SIZE_UPLOAD = CURLINFOTYPE.DOUBLE + 7,
        SIZE_UPLOAD_T = CURLINFOTYPE.OFF_T + 7,
        SIZE_DOWNLOAD = CURLINFOTYPE.DOUBLE + 8,
        SIZE_DOWNLOAD_T = CURLINFOTYPE.OFF_T + 8,
        SPEED_DOWNLOAD = CURLINFOTYPE.DOUBLE + 9,
        SPEED_DOWNLOAD_T = CURLINFOTYPE.OFF_T + 9,
        SPEED_UPLOAD = CURLINFOTYPE.DOUBLE + 10,
        SPEED_UPLOAD_T = CURLINFOTYPE.OFF_T + 10,
        HEADER_SIZE = CURLINFOTYPE.LONG + 11,
        REQUEST_SIZE = CURLINFOTYPE.LONG + 12,
        SSL_VERIFYRESULT = CURLINFOTYPE.LONG + 13,
        FILETIME = CURLINFOTYPE.LONG + 14,
        CONTENT_LENGTH_DOWNLOAD = CURLINFOTYPE.DOUBLE + 15,
        CONTENT_LENGTH_DOWNLOAD_T = CURLINFOTYPE.OFF_T + 15,
        CONTENT_LENGTH_UPLOAD = CURLINFOTYPE.DOUBLE + 16,
        CONTENT_LENGTH_UPLOAD_T = CURLINFOTYPE.OFF_T + 16,
        STARTTRANSFER_TIME = CURLINFOTYPE.DOUBLE + 17,
        CONTENT_TYPE = CURLINFOTYPE.STRING + 18,
        REDIRECT_TIME = CURLINFOTYPE.DOUBLE + 19,
        REDIRECT_COUNT = CURLINFOTYPE.LONG + 20,
        PRIVATE = CURLINFOTYPE.STRING + 21,
        HTTP_CONNECTCODE = CURLINFOTYPE.LONG + 22,
        HTTPAUTH_AVAIL = CURLINFOTYPE.LONG + 23,
        PROXYAUTH_AVAIL = CURLINFOTYPE.LONG + 24,
        OS_ERRNO = CURLINFOTYPE.LONG + 25,
        NUM_CONNECTS = CURLINFOTYPE.LONG + 26,
        SSL_ENGINES = CURLINFOTYPE.SLIST + 27,
        COOKIELIST = CURLINFOTYPE.SLIST + 28,
        LASTSOCKET = CURLINFOTYPE.LONG + 29,
        FTP_ENTRY_PATH = CURLINFOTYPE.STRING + 30,
        REDIRECT_URL = CURLINFOTYPE.STRING + 31,
        PRIMARY_IP = CURLINFOTYPE.STRING + 32,
        APPCONNECT_TIME = CURLINFOTYPE.DOUBLE + 33,
        CERTINFO = CURLINFOTYPE.PTR + 34,
        CONDITION_UNMET = CURLINFOTYPE.LONG + 35,
        RTSP_SESSION_ID = CURLINFOTYPE.STRING + 36,
        RTSP_CLIENT_CSEQ = CURLINFOTYPE.LONG + 37,
        RTSP_SERVER_CSEQ = CURLINFOTYPE.LONG + 38,
        RTSP_CSEQ_RECV = CURLINFOTYPE.LONG + 39,
        PRIMARY_PORT = CURLINFOTYPE.LONG + 40,
        LOCAL_IP = CURLINFOTYPE.STRING + 41,
        LOCAL_PORT = CURLINFOTYPE.LONG + 42,
        TLS_SESSION = CURLINFOTYPE.PTR + 43,
        ACTIVESOCKET = CURLINFOTYPE.SOCKET + 44,
        TLS_SSL_PTR = CURLINFOTYPE.PTR + 45,
        HTTP_VERSION = CURLINFOTYPE.LONG + 46,
        PROXY_SSL_VERIFYRESULT = CURLINFOTYPE.LONG + 47,
        PROTOCOL = CURLINFOTYPE.LONG + 48,
        SCHEME = CURLINFOTYPE.STRING + 49,
        // Fill in new entries below here!

        LASTONE = 49
    }

    /// <summary>
    ///     Reference: https://github.com/curl/curl/blob/master/include/curl/multi.h
    /// </summary>
    public enum CURLMcode
    {
        /// <summary>
        ///     Please call curl_multi_perform() or curl_multi_socket*() soon.
        /// </summary>
        CALL_MULTI_PERFORM = -1,

        OK,

        /// <summary>
        ///     The passed-in handle is not a valid CURLM handle.
        /// </summary>
        BAD_HANDLE,

        /// <summary>
        ///     An easy handle was not good/valid.
        /// </summary>
        BAD_EASY_HANDLE,

        /// <summary>
        ///     If you ever get this, you're in deep sh*t.
        /// </summary>
        OUT_OF_MEMORY,

        /// <summary>
        ///     This is a libcurl bug.
        /// </summary>
        INTERNAL_ERROR,

        /// <summary>
        ///     The passed in socket argument did not match.
        /// </summary>
        BAD_SOCKET,

        /// <summary>
        ///     curl_multi_setopt() with unsupported option.
        /// </summary>
        UNKNOWN_OPTION,

        /// <summary>
        ///     An easy handle already added to a multi handle was attempted to get added - again.
        /// </summary>
        ADDED_ALREADY,

        LAST
    }

    internal static class CURLOPTTYPE
    {
        public const uint LONG = 0;
        public const uint OBJECTPOINT = 10000;
        public const uint STRINGPOINT = 10000;
        public const uint FUNCTIONPOINT = 20000;
        public const uint OFF_T = 30000;
    }

    public enum CURLoption : uint
    {
        /* This is the FILE * or void * the regular output should be written to. */
        WRITEDATA = CURLOPTTYPE.OBJECTPOINT + 1,

        /* The full URL to get/put */
        URL = CURLOPTTYPE.STRINGPOINT + 2,

        /* Port number to connect to, if other than default. */
        PORT = CURLOPTTYPE.LONG + 3,

        /* Name of proxy to use. */
        PROXY = CURLOPTTYPE.STRINGPOINT + 4,

        /* "user:password;options" to use when fetching. */
        USERPWD = CURLOPTTYPE.STRINGPOINT + 5,

        /* "user:password" to use with proxy. */
        PROXYUSERPWD = CURLOPTTYPE.STRINGPOINT + 6,

        /* Range to get, specified as an ASCII string. */
        RANGE = CURLOPTTYPE.STRINGPOINT + 7,

        /* not used */

        /* Specified file stream to upload from (use as input): */
        READDATA = CURLOPTTYPE.OBJECTPOINT + 9,

        /* Buffer to receive error messages in, must be at least CURL_ERROR_SIZE
         * bytes big. If this is not used, error messages go to stderr instead: */
        ERRORBUFFER = CURLOPTTYPE.OBJECTPOINT + 10,

        /* Function that will be called to store the output (instead of fwrite). The
         * parameters will use fwrite() syntax, make sure to follow them. */
        WRITEFUNCTION = CURLOPTTYPE.FUNCTIONPOINT + 11,

        /* Function that will be called to read the input (instead of fread). The
         * parameters will use fread() syntax, make sure to follow them. */
        READFUNCTION = CURLOPTTYPE.FUNCTIONPOINT + 12,

        /* Time-out the read operation after this amount of seconds */
        TIMEOUT = CURLOPTTYPE.LONG + 13,

        /* If the CURLOPT_INFILE is used, this can be used to inform libcurl about
         * how large the file being sent really is. That allows better error
         * checking and better verifies that the upload was successful. -1 means
         * unknown size.
         *
         * For large file support, there is also a _LARGE version of the key
         * which takes an off_t type, allowing platforms with larger off_t
         * sizes to handle larger files.  See below for INFILESIZE_LARGE.
         */
        INFILESIZE = CURLOPTTYPE.LONG + 14,

        /* POST static input fields. */
        POSTFIELDS = CURLOPTTYPE.OBJECTPOINT + 15,

        /* Set the referrer page (needed by some CGIs) */
        REFERER = CURLOPTTYPE.STRINGPOINT + 16,

        /* Set the FTP PORT string (interface name, named or numerical IP address)
           Use i.e '-' to use default address. */
        FTPPORT = CURLOPTTYPE.STRINGPOINT + 17,

        /* Set the User-Agent string (examined by some CGIs) */
        USERAGENT = CURLOPTTYPE.STRINGPOINT + 18,

        /* If the download receives less than "low speed limit" bytes/second
         * during "low speed time" seconds, the operations is aborted.
         * You could i.e if you have a pretty high speed connection, abort if
         * it is less than 2000 bytes/sec during 20 seconds.
         */

        /* Set the "low speed limit" */
        LOW_SPEED_LIMIT = CURLOPTTYPE.LONG + 19,

        /* Set the "low speed time" */
        LOW_SPEED_TIME = CURLOPTTYPE.LONG + 20,

        /* Set the continuation offset.
         *
         * Note there is also a _LARGE version of this key which uses
         * off_t types, allowing for large file offsets on platforms which
         * use larger-than-32-bit off_t's.  Look below for RESUME_FROM_LARGE.
         */
        RESUME_FROM = CURLOPTTYPE.LONG + 21,

        /* Set cookie in request: */
        COOKIE = CURLOPTTYPE.STRINGPOINT + 22,

        /* This points to a linked list of headers, struct curl_slist kind. This
           list is also used for RTSP (in spite of its name) */
        HTTPHEADER = CURLOPTTYPE.OBJECTPOINT + 23,

        /* This points to a linked list of post entries, struct curl_httppost */
        HTTPPOST = CURLOPTTYPE.OBJECTPOINT + 24,

        /* name of the file keeping your private SSL-certificate */
        SSLCERT = CURLOPTTYPE.STRINGPOINT + 25,

        /* password for the SSL or SSH private key */
        KEYPASSWD = CURLOPTTYPE.STRINGPOINT + 26,

        /* send TYPE parameter? */
        CRLF = CURLOPTTYPE.LONG + 27,

        /* send linked-list of QUOTE commands */
        QUOTE = CURLOPTTYPE.OBJECTPOINT + 28,

        /* send FILE * or void * to store headers to, if you use a callback it
           is simply passed to the callback unmodified */
        HEADERDATA = CURLOPTTYPE.OBJECTPOINT + 29,

        /* point to a file to read the initial cookies from, also enables
           "cookie awareness" */
        COOKIEFILE = CURLOPTTYPE.STRINGPOINT + 31,

        /* What version to specifically try to use.
           See CURL_SSLVERSION defines below. */
        SSLVERSION = CURLOPTTYPE.LONG + 32,

        /* What kind of HTTP time condition to use, see defines */
        TIMECONDITION = CURLOPTTYPE.LONG + 33,

        /* Time to use with the above condition. Specified in number of seconds
           since 1 Jan 1970 */
        TIMEVALUE = CURLOPTTYPE.LONG + 34,

        /* 35 = OBSOLETE */

        /* Custom request, for customizing the get command like
           HTTP: DELETE, TRACE and others
           FTP: to use a different list command
           */
        CUSTOMREQUEST = CURLOPTTYPE.STRINGPOINT + 36,

        /* FILE handle to use instead of stderr */
        STDERR = CURLOPTTYPE.OBJECTPOINT + 37,

        /* 38 is not used */

        /* send linked-list of post-transfer QUOTE commands */
        POSTQUOTE = CURLOPTTYPE.OBJECTPOINT + 39,

        OBSOLETE40 = CURLOPTTYPE.OBJECTPOINT + 40, /* OBSOLETE, do not use! */

        VERBOSE = CURLOPTTYPE.LONG + 41,      /* talk a lot */
        HEADER = CURLOPTTYPE.LONG + 42,       /* throw the header out too */
        NOPROGRESS = CURLOPTTYPE.LONG + 43,   /* shut off the progress meter */
        NOBODY = CURLOPTTYPE.LONG + 44,       /* use HEAD to get http document */
        FAILONERROR = CURLOPTTYPE.LONG + 45,  /* no output on http error codes >= 400 */
        UPLOAD = CURLOPTTYPE.LONG + 46,       /* this is an upload */
        POST = CURLOPTTYPE.LONG + 47,         /* HTTP POST method */
        DIRLISTONLY = CURLOPTTYPE.LONG + 48,  /* bare names when listing directories */

        APPEND = CURLOPTTYPE.LONG + 50,       /* Append instead of overwrite on upload! */

        /* Specify whether to read the user+password from the .netrc or the URL.
         * This must be one of the CURL_NETRC_* enums below. */
        NETRC = CURLOPTTYPE.LONG + 51,

        FOLLOWLOCATION = CURLOPTTYPE.LONG + 52,  /* use Location: Luke! */

        TRANSFERTEXT = CURLOPTTYPE.LONG + 53, /* transfer data in text/ASCII format */
        PUT = CURLOPTTYPE.LONG + 54,          /* HTTP PUT */

        /* 55 = OBSOLETE */

        /* DEPRECATED
         * Function that will be called instead of the internal progress display
         * function. This function should be defined as the curl_progress_callback
         * prototype defines. */
        PROGRESSFUNCTION = CURLOPTTYPE.FUNCTIONPOINT + 56,

        /* Data passed to the CURLOPT_PROGRESSFUNCTION and CURLOPT_XFERINFOFUNCTION
           callbacks */
        PROGRESSDATA = CURLOPTTYPE.OBJECTPOINT + 57,

        /* We want the referrer field set automatically when following locations */
        AUTOREFERER = CURLOPTTYPE.LONG + 58,

        /* Port of the proxy, can be set in the proxy string as well with:
           "[host]:[port]" */
        PROXYPORT = CURLOPTTYPE.LONG + 59,

        /* size of the POST input data, if strlen() is not good to use */
        POSTFIELDSIZE = CURLOPTTYPE.LONG + 60,

        /* tunnel non-http operations through a HTTP proxy */
        HTTPPROXYTUNNEL = CURLOPTTYPE.LONG + 61,

        /* Set the interface string to use as outgoing network interface */
        INTERFACE = CURLOPTTYPE.STRINGPOINT + 62,

        /* Set the krb4/5 security level, this also enables krb4/5 awareness.  This
         * is a string, 'clear', 'safe', 'confidential' or 'private'.  If the string
         * is set but doesn't match one of these, 'private' will be used.  */
        KRBLEVEL = CURLOPTTYPE.STRINGPOINT + 63,

        /* Set if we should verify the peer in ssl handshake, set 1 to verify. */
        SSL_VERIFYPEER = CURLOPTTYPE.LONG + 64,

        /* The CApath or CAfile used to validate the peer certificate
           this option is used only if SSL_VERIFYPEER is true */
        CAINFO = CURLOPTTYPE.STRINGPOINT + 65,

        /* 66 = OBSOLETE */
        /* 67 = OBSOLETE */

        /* Maximum number of http redirects to follow */
        MAXREDIRS = CURLOPTTYPE.LONG + 68,

        /* Pass a long set to 1 to get the date of the requested document (if
           possible)! Pass a zero to shut it off. */
        FILETIME = CURLOPTTYPE.LONG + 69,

        /* This points to a linked list of telnet options */
        TELNETOPTIONS = CURLOPTTYPE.OBJECTPOINT + 70,

        /* Max amount of cached alive connections */
        MAXCONNECTS = CURLOPTTYPE.LONG + 71,

        OBSOLETE72 = CURLOPTTYPE.LONG + 72, /* OBSOLETE, do not use! */

        /* 73 = OBSOLETE */

        /* Set to explicitly use a new connection for the upcoming transfer.
           Do not use this unless you're absolutely sure of this, as it makes the
           operation slower and is less friendly for the network. */
        FRESH_CONNECT = CURLOPTTYPE.LONG + 74,

        /* Set to explicitly forbid the upcoming transfer's connection to be re-used
           when done. Do not use this unless you're absolutely sure of this, as it
           makes the operation slower and is less friendly for the network. */
        FORBID_REUSE = CURLOPTTYPE.LONG + 75,

        /* Set to a file name that contains random data for libcurl to use to
           seed the random engine when doing SSL connects. */
        RANDOM_FILE = CURLOPTTYPE.STRINGPOINT + 76,

        /* Set to the Entropy Gathering Daemon socket pathname */
        EGDSOCKET = CURLOPTTYPE.STRINGPOINT + 77,

        /* Time-out connect operations after this amount of seconds, if connects are
           OK within this time, then fine... This only aborts the connect phase. */
        CONNECTTIMEOUT = CURLOPTTYPE.LONG + 78,

        /* Function that will be called to store headers (instead of fwrite). The
         * parameters will use fwrite() syntax, make sure to follow them. */
        HEADERFUNCTION = CURLOPTTYPE.FUNCTIONPOINT + 79,

        /* Set this to force the HTTP request to get back to GET. Only really usable
           if POST, PUT or a custom request have been used first.
         */
        HTTPGET = CURLOPTTYPE.LONG + 80,

        /* Set if we should verify the Common name from the peer certificate in ssl
         * handshake, set 1 to check existence, 2 to ensure that it matches the
         * provided hostname. */
        SSL_VERIFYHOST = CURLOPTTYPE.LONG + 81,

        /* Specify which file name to write all known cookies in after completed
           operation. Set file name to "-" (dash) to make it go to stdout. */
        COOKIEJAR = CURLOPTTYPE.STRINGPOINT + 82,

        /* Specify which SSL ciphers to use */
        SSL_CIPHER_LIST = CURLOPTTYPE.STRINGPOINT + 83,

        /* Specify which HTTP version to use! This must be set to one of the
           CURL_HTTP_VERSION* enums set below. */
        HTTP_VERSION = CURLOPTTYPE.LONG + 84,

        /* Specifically switch on or off the FTP engine's use of the EPSV command. By
           default, that one will always be attempted before the more traditional
           PASV command. */
        FTP_USE_EPSV = CURLOPTTYPE.LONG + 85,

        /* type of the file keeping your SSL-certificate ("DER", "PEM", "ENG") */
        SSLCERTTYPE = CURLOPTTYPE.STRINGPOINT + 86,

        /* name of the file keeping your private SSL-key */
        SSLKEY = CURLOPTTYPE.STRINGPOINT + 87,

        /* type of the file keeping your private SSL-key ("DER", "PEM", "ENG") */
        SSLKEYTYPE = CURLOPTTYPE.STRINGPOINT + 88,

        /* crypto engine for the SSL-sub system */
        SSLENGINE = CURLOPTTYPE.STRINGPOINT + 89,

        /* set the crypto engine for the SSL-sub system as default
           the param has no meaning...
         */
        SSLENGINE_DEFAULT = CURLOPTTYPE.LONG + 90,

        /* Non-zero value means to use the global dns cache */
        DNS_USE_GLOBAL_CACHE = CURLOPTTYPE.LONG + 91, /* DEPRECATED, do not use! */

        /* DNS cache timeout */
        DNS_CACHE_TIMEOUT = CURLOPTTYPE.LONG + 92,

        /* send linked-list of pre-transfer QUOTE commands */
        PREQUOTE = CURLOPTTYPE.OBJECTPOINT + 93,

        /* set the debug function */
        DEBUGFUNCTION = CURLOPTTYPE.FUNCTIONPOINT + 94,

        /* set the data for the debug function */
        DEBUGDATA = CURLOPTTYPE.OBJECTPOINT + 95,

        /* mark this as start of a cookie session */
        COOKIESESSION = CURLOPTTYPE.LONG + 96,

        /* The CApath directory used to validate the peer certificate
           this option is used only if SSL_VERIFYPEER is true */
        CAPATH = CURLOPTTYPE.STRINGPOINT + 97,

        /* Instruct libcurl to use a smaller receive buffer */
        BUFFERSIZE = CURLOPTTYPE.LONG + 98,

        /* Instruct libcurl to not use any signal/alarm handlers, even when using
           timeouts. This option is useful for multi-threaded applications.
           See libcurl-the-guide for more background information. */
        NOSIGNAL = CURLOPTTYPE.LONG + 99,

        /* Provide a CURLShare for mutexing non-ts data */
        SHARE = CURLOPTTYPE.OBJECTPOINT + 100,

        /* indicates type of proxy. accepted values are CURLPROXY_HTTP (default),
           CURLPROXY_HTTPS, CURLPROXY_SOCKS4, CURLPROXY_SOCKS4A and
           CURLPROXY_SOCKS5. */
        PROXYTYPE = CURLOPTTYPE.LONG + 101,

        /* Set the Accept-Encoding string. Use this to tell a server you would like
           the response to be compressed. Before 7.21.6, this was known as
           CURLOPT_ENCODING */
        ACCEPT_ENCODING = CURLOPTTYPE.STRINGPOINT + 102,

        /* Set pointer to private data */
        PRIVATE = CURLOPTTYPE.OBJECTPOINT + 103,

        /* Set aliases for HTTP 200 in the HTTP Response header */
        HTTP200ALIASES = CURLOPTTYPE.OBJECTPOINT + 104,

        /* Continue to send authentication (user+password) when following locations,
           even when hostname changed. This can potentially send off the name
           and password to whatever host the server decides. */
        UNRESTRICTED_AUTH = CURLOPTTYPE.LONG + 105,

        /* Specifically switch on or off the FTP engine's use of the EPRT command (
           it also disables the LPRT attempt). By default, those ones will always be
           attempted before the good old traditional PORT command. */
        FTP_USE_EPRT = CURLOPTTYPE.LONG + 106,

        /* Set this to a bitmask value to enable the particular authentications
           methods you like. Use this in combination with CURLOPT_USERPWD.
           Note that setting multiple bits may cause extra network round-trips. */
        HTTPAUTH = CURLOPTTYPE.LONG + 107,

        /* Set the ssl context callback function, currently only for OpenSSL ssl_ctx
           in second argument. The function must be matching the
           curl_ssl_ctx_callback proto. */
        SSL_CTX_FUNCTION = CURLOPTTYPE.FUNCTIONPOINT + 108,

        /* Set the userdata for the ssl context callback function's third
           argument */
        SSL_CTX_DATA = CURLOPTTYPE.OBJECTPOINT + 109,

        /* FTP Option that causes missing dirs to be created on the remote server.
           In 7.19.4 we introduced the convenience enums for this option using the
           CURLFTP_CREATE_DIR prefix.
        */
        FTP_CREATE_MISSING_DIRS = CURLOPTTYPE.LONG + 110,

        /* Set this to a bitmask value to enable the particular authentications
           methods you like. Use this in combination with CURLOPT_PROXYUSERPWD.
           Note that setting multiple bits may cause extra network round-trips. */
        PROXYAUTH = CURLOPTTYPE.LONG + 111,

        /* FTP option that changes the timeout, in seconds, associated with
           getting a response.  This is different from transfer timeout time and
           essentially places a demand on the FTP server to acknowledge commands
           in a timely manner. */
        FTP_RESPONSE_TIMEOUT = CURLOPTTYPE.LONG + 112,

        /* Set this option to one of the CURL_IPRESOLVE_* defines (see below) to
           tell libcurl to resolve names to those IP versions only. This only has
           affect on systems with support for more than one, i.e IPv4 _and_ IPv6. */
        IPRESOLVE = CURLOPTTYPE.LONG + 113,

        /* Set this option to limit the size of a file that will be downloaded from
           an HTTP or FTP server.
           Note there is also _LARGE version which adds large file support for
           platforms which have larger off_t sizes.  See MAXFILESIZE_LARGE below. */
        MAXFILESIZE = CURLOPTTYPE.LONG + 114,

        /* See the comment for INFILESIZE above, but in short, specifies
         * the size of the file being uploaded.  -1 means unknown.
         */
        INFILESIZE_LARGE = CURLOPTTYPE.OFF_T + 115,

        /* Sets the continuation offset.  There is also a LONG version of this;
         * look above for RESUME_FROM.
         */
        RESUME_FROM_LARGE = CURLOPTTYPE.OFF_T + 116,

        /* Sets the maximum size of data that will be downloaded from
         * an HTTP or FTP server.  See MAXFILESIZE above for the LONG version.
         */
        MAXFILESIZE_LARGE = CURLOPTTYPE.OFF_T + 117,

        /* Set this option to the file name of your .netrc file you want libcurl
           to parse (using the CURLOPT_NETRC option). If not set, libcurl will do
           a poor attempt to find the user's home directory and check for a .netrc
           file in there. */
        NETRC_FILE = CURLOPTTYPE.STRINGPOINT + 118,

        /* Enable SSL/TLS for FTP, pick one of:
           CURLUSESSL_TRY     - try using SSL, proceed anyway otherwise
           CURLUSESSL_CONTROL - SSL for the control connection or fail
           CURLUSESSL_ALL     - SSL for all communication or fail
        */
        USE_SSL = CURLOPTTYPE.LONG + 119,

        /* The _LARGE version of the standard POSTFIELDSIZE option */
        POSTFIELDSIZE_LARGE = CURLOPTTYPE.OFF_T + 120,

        /* Enable/disable the TCP Nagle algorithm */
        TCP_NODELAY = CURLOPTTYPE.LONG + 121,

        /* 122 OBSOLETE, used in 7.12.3. Gone in 7.13.0 */
        /* 123 OBSOLETE. Gone in 7.16.0 */
        /* 124 OBSOLETE, used in 7.12.3. Gone in 7.13.0 */
        /* 125 OBSOLETE, used in 7.12.3. Gone in 7.13.0 */
        /* 126 OBSOLETE, used in 7.12.3. Gone in 7.13.0 */
        /* 127 OBSOLETE. Gone in 7.16.0 */
        /* 128 OBSOLETE. Gone in 7.16.0 */

        /* When FTP over SSL/TLS is selected (with CURLOPT_USE_SSL), this option
           can be used to change libcurl's default action which is to first try
           "AUTH SSL" and then "AUTH TLS" in this order, and proceed when a OK
           response has been received.
           Available parameters are:
           CURLFTPAUTH_DEFAULT - let libcurl decide
           CURLFTPAUTH_SSL     - try "AUTH SSL" first, then TLS
           CURLFTPAUTH_TLS     - try "AUTH TLS" first, then SSL
        */
        FTPSSLAUTH = CURLOPTTYPE.LONG + 129,

        IOCTLFUNCTION = CURLOPTTYPE.FUNCTIONPOINT + 130,
        IOCTLDATA = CURLOPTTYPE.OBJECTPOINT + 131,

        /* 132 OBSOLETE. Gone in 7.16.0 */
        /* 133 OBSOLETE. Gone in 7.16.0 */

        /* zero terminated string for pass on to the FTP server when asked for
           "account" info */
        FTP_ACCOUNT = CURLOPTTYPE.STRINGPOINT + 134,

        /* feed cookie into cookie engine */
        COOKIELIST = CURLOPTTYPE.STRINGPOINT + 135,

        /* ignore Content-Length */
        IGNORE_CONTENT_LENGTH = CURLOPTTYPE.LONG + 136,

        /* Set to non-zero to skip the IP address received in a 227 PASV FTP server
           response. Typically used for FTP-SSL purposes but is not restricted to
           that. libcurl will then instead use the same IP address it used for the
           control connection. */
        FTP_SKIP_PASV_IP = CURLOPTTYPE.LONG + 137,

        /* Select "file method" to use when doing FTP, see the curl_ftpmethod
           above. */
        FTP_FILEMETHOD = CURLOPTTYPE.LONG + 138,

        /* Local port number to bind the socket to */
        LOCALPORT = CURLOPTTYPE.LONG + 139,

        /* Number of ports to try, including the first one set with LOCALPORT.
           Thus, setting it to 1 will make no additional attempts but the first.
        */
        LOCALPORTRANGE = CURLOPTTYPE.LONG + 140,

        /* no transfer, set up connection and let application use the socket by
           extracting it with CURLINFO_LASTSOCKET */
        CONNECT_ONLY = CURLOPTTYPE.LONG + 141,

        /* Function that will be called to convert from the
           network encoding (instead of using the iconv calls in libcurl) */
        CONV_FROM_NETWORK_FUNCTION = CURLOPTTYPE.FUNCTIONPOINT + 142,

        /* Function that will be called to convert to the
           network encoding (instead of using the iconv calls in libcurl) */
        CONV_TO_NETWORK_FUNCTION = CURLOPTTYPE.FUNCTIONPOINT + 143,

        /* Function that will be called to convert from UTF8
           (instead of using the iconv calls in libcurl)
           Note that this is used only for SSL certificate processing */
        CONV_FROM_UTF8_FUNCTION = CURLOPTTYPE.FUNCTIONPOINT + 144,

        /* if the connection proceeds too quickly then need to slow it down */
        /* limit-rate: maximum number of bytes per second to send or receive */
        MAX_SEND_SPEED_LARGE = CURLOPTTYPE.OFF_T + 145,
        MAX_RECV_SPEED_LARGE = CURLOPTTYPE.OFF_T + 146,

        /* Pointer to command string to send if USER/PASS fails. */
        FTP_ALTERNATIVE_TO_USER = CURLOPTTYPE.STRINGPOINT + 147,

        /* callback function for setting socket options */
        SOCKOPTFUNCTION = CURLOPTTYPE.FUNCTIONPOINT + 148,
        SOCKOPTDATA = CURLOPTTYPE.OBJECTPOINT + 149,

        /* set to 0 to disable session ID re-use for this transfer, default is
           enabled (== 1) */
        SSL_SESSIONID_CACHE = CURLOPTTYPE.LONG + 150,

        /* allowed SSH authentication methods */
        SSH_AUTH_TYPES = CURLOPTTYPE.LONG + 151,

        /* Used by scp/sftp to do public/private key authentication */
        SSH_PUBLIC_KEYFILE = CURLOPTTYPE.STRINGPOINT + 152,
        SSH_PRIVATE_KEYFILE = CURLOPTTYPE.STRINGPOINT + 153,

        /* Send CCC (Clear Command Channel) after authentication */
        FTP_SSL_CCC = CURLOPTTYPE.LONG + 154,

        /* Same as TIMEOUT and CONNECTTIMEOUT, but with ms resolution */
        TIMEOUT_MS = CURLOPTTYPE.LONG + 155,
        CONNECTTIMEOUT_MS = CURLOPTTYPE.LONG + 156,

        /* set to zero to disable the libcurl's decoding and thus pass the raw body
           data to the application even when it is encoded/compressed */
        HTTP_TRANSFER_DECODING = CURLOPTTYPE.LONG + 157,
        HTTP_CONTENT_DECODING = CURLOPTTYPE.LONG + 158,

        /* Permission used when creating new files and directories on the remote
           server for protocols that support it, SFTP/SCP/FILE */
        NEW_FILE_PERMS = CURLOPTTYPE.LONG + 159,
        NEW_DIRECTORY_PERMS = CURLOPTTYPE.LONG + 160,

        /* Set the behaviour of POST when redirecting. Values must be set to one
           of CURL_REDIR* defines below. This used to be called CURLOPT_POST301 */
        POSTREDIR = CURLOPTTYPE.LONG + 161,

        /* used by scp/sftp to verify the host's public key */
        SSH_HOST_PUBLIC_KEY_MD5 = CURLOPTTYPE.STRINGPOINT + 162,

        /* Callback function for opening socket (instead of socket(2)). Optionally,
           callback is able change the address or refuse to connect returning
           CURL_SOCKET_BAD.  The callback should have type
           curl_opensocket_callback */
        OPENSOCKETFUNCTION = CURLOPTTYPE.FUNCTIONPOINT + 163,
        OPENSOCKETDATA = CURLOPTTYPE.OBJECTPOINT + 164,

        /* POST volatile input fields. */
        COPYPOSTFIELDS = CURLOPTTYPE.OBJECTPOINT + 165,

        /* set transfer mode (;type=<a|i>) when doing FTP via an HTTP proxy */
        PROXY_TRANSFER_MODE = CURLOPTTYPE.LONG + 166,

        /* Callback function for seeking in the input stream */
        SEEKFUNCTION = CURLOPTTYPE.FUNCTIONPOINT + 167,
        SEEKDATA = CURLOPTTYPE.OBJECTPOINT + 168,

        /* CRL file */
        CRLFILE = CURLOPTTYPE.STRINGPOINT + 169,

        /* Issuer certificate */
        ISSUERCERT = CURLOPTTYPE.STRINGPOINT + 170,

        /* (IPv6) Address scope */
        ADDRESS_SCOPE = CURLOPTTYPE.LONG + 171,

        /* Collect certificate chain info and allow it to get retrievable with
           CURLINFO_CERTINFO after the transfer is complete. */
        CERTINFO = CURLOPTTYPE.LONG + 172,

        /* "name" and "pwd" to use when fetching. */
        USERNAME = CURLOPTTYPE.STRINGPOINT + 173,
        PASSWORD = CURLOPTTYPE.STRINGPOINT + 174,

        /* "name" and "pwd" to use with Proxy when fetching. */
        PROXYUSERNAME = CURLOPTTYPE.STRINGPOINT + 175,
        PROXYPASSWORD = CURLOPTTYPE.STRINGPOINT + 176,

        /* Comma separated list of hostnames defining no-proxy zones. These should
           match both hostnames directly, and hostnames within a domain. For
           example, local.com will match local.com and www.local.com, but NOT
           notlocal.com or www.notlocal.com. For compatibility with other
           implementations of this, .local.com will be considered to be the same as
           local.com. A single * is the only valid wildcard, and effectively
           disables the use of proxy. */
        NOPROXY = CURLOPTTYPE.STRINGPOINT + 177,

        /* block size for TFTP transfers */
        TFTP_BLKSIZE = CURLOPTTYPE.LONG + 178,

        /* Socks Service */
        SOCKS5_GSSAPI_SERVICE = CURLOPTTYPE.STRINGPOINT + 179, /* DEPRECATED, do not use! */

        /* Socks Service */
        SOCKS5_GSSAPI_NEC = CURLOPTTYPE.LONG + 180,

        /* set the bitmask for the protocols that are allowed to be used for the
           transfer, which thus helps the app which takes URLs from users or other
           external inputs and want to restrict what protocol(s) to deal
           with. Defaults to CURLPROTO_ALL. */
        PROTOCOLS = CURLOPTTYPE.LONG + 181,

        /* set the bitmask for the protocols that libcurl is allowed to follow to,
           as a subset of the CURLOPT_PROTOCOLS ones. That means the protocol needs
           to be set in both bitmasks to be allowed to get redirected to. Defaults
           to all protocols except FILE and SCP. */
        REDIR_PROTOCOLS = CURLOPTTYPE.LONG + 182,

        /* set the SSH knownhost file name to use */
        SSH_KNOWNHOSTS = CURLOPTTYPE.STRINGPOINT + 183,

        /* set the SSH host key callback, must point to a curl_sshkeycallback
           function */
        SSH_KEYFUNCTION = CURLOPTTYPE.FUNCTIONPOINT + 184,

        /* set the SSH host key callback custom pointer */
        SSH_KEYDATA = CURLOPTTYPE.OBJECTPOINT + 185,

        /* set the SMTP mail originator */
        MAIL_FROM = CURLOPTTYPE.STRINGPOINT + 186,

        /* set the list of SMTP mail receiver(s) */
        MAIL_RCPT = CURLOPTTYPE.OBJECTPOINT + 187,

        /* FTP: send PRET before PASV */
        FTP_USE_PRET = CURLOPTTYPE.LONG + 188,

        /* RTSP request method (OPTIONS, SETUP, PLAY, etc...) */
        RTSP_REQUEST = CURLOPTTYPE.LONG + 189,

        /* The RTSP session identifier */
        RTSP_SESSION_ID = CURLOPTTYPE.STRINGPOINT + 190,

        /* The RTSP stream URI */
        RTSP_STREAM_URI = CURLOPTTYPE.STRINGPOINT + 191,

        /* The Transport: header to use in RTSP requests */
        RTSP_TRANSPORT = CURLOPTTYPE.STRINGPOINT + 192,

        /* Manually initialize the client RTSP CSeq for this handle */
        RTSP_CLIENT_CSEQ = CURLOPTTYPE.LONG + 193,

        /* Manually initialize the server RTSP CSeq for this handle */
        RTSP_SERVER_CSEQ = CURLOPTTYPE.LONG + 194,

        /* The stream to pass to INTERLEAVEFUNCTION. */
        INTERLEAVEDATA = CURLOPTTYPE.OBJECTPOINT + 195,

        /* Let the application define a custom write method for RTP data */
        INTERLEAVEFUNCTION = CURLOPTTYPE.FUNCTIONPOINT + 196,

        /* Turn on wildcard matching */
        WILDCARDMATCH = CURLOPTTYPE.LONG + 197,

        /* Directory matching callback called before downloading of an
           individual file (chunk) started */
        CHUNK_BGN_FUNCTION = CURLOPTTYPE.FUNCTIONPOINT + 198,

        /* Directory matching callback called after the file (chunk)
           was downloaded, or skipped */
        CHUNK_END_FUNCTION = CURLOPTTYPE.FUNCTIONPOINT + 199,

        /* Change match (fnmatch-like) callback for wildcard matching */
        FNMATCH_FUNCTION = CURLOPTTYPE.FUNCTIONPOINT + 200,

        /* Let the application define custom chunk data pointer */
        CHUNK_DATA = CURLOPTTYPE.OBJECTPOINT + 201,

        /* FNMATCH_FUNCTION user pointer */
        FNMATCH_DATA = CURLOPTTYPE.OBJECTPOINT + 202,

        /* send linked-list of name:port:address sets */
        RESOLVE = CURLOPTTYPE.OBJECTPOINT + 203,

        /* Set a username for authenticated TLS */
        TLSAUTH_USERNAME = CURLOPTTYPE.STRINGPOINT + 204,

        /* Set a password for authenticated TLS */
        TLSAUTH_PASSWORD = CURLOPTTYPE.STRINGPOINT + 205,

        /* Set authentication type for authenticated TLS */
        TLSAUTH_TYPE = CURLOPTTYPE.STRINGPOINT + 206,

        /* Set to 1 to enable the "TE:" header in HTTP requests to ask for
           compressed transfer-encoded responses. Set to 0 to disable the use of TE:
           in outgoing requests. The current default is 0, but it might change in a
           future libcurl release.
           libcurl will ask for the compressed methods it knows of, and if that
           isn't any, it will not ask for transfer-encoding at all even if this
           option is set to 1.
        */
        TRANSFER_ENCODING = CURLOPTTYPE.LONG + 207,

        /* Callback function for closing socket (instead of close(2)). The callback
           should have type curl_closesocket_callback */
        CLOSESOCKETFUNCTION = CURLOPTTYPE.FUNCTIONPOINT + 208,
        CLOSESOCKETDATA = CURLOPTTYPE.OBJECTPOINT + 209,

        /* allow GSSAPI credential delegation */
        GSSAPI_DELEGATION = CURLOPTTYPE.LONG + 210,

        /* Set the name servers to use for DNS resolution */
        DNS_SERVERS = CURLOPTTYPE.STRINGPOINT + 211,

        /* Time-out accept operations (currently for FTP only) after this amount
           of milliseconds. */
        ACCEPTTIMEOUT_MS = CURLOPTTYPE.LONG + 212,

        /* Set TCP keepalive */
        TCP_KEEPALIVE = CURLOPTTYPE.LONG + 213,

        /* non-universal keepalive knobs (Linux, AIX, HP-UX, more) */
        TCP_KEEPIDLE = CURLOPTTYPE.LONG + 214,
        TCP_KEEPINTVL = CURLOPTTYPE.LONG + 215,

        /* Enable/disable specific SSL features with a bitmask, see CURLSSLOPT_* */
        SSL_OPTIONS = CURLOPTTYPE.LONG + 216,

        /* Set the SMTP auth originator */
        MAIL_AUTH = CURLOPTTYPE.STRINGPOINT + 217,

        /* Enable/disable SASL initial response */
        SASL_IR = CURLOPTTYPE.LONG + 218,

        /* Function that will be called instead of the internal progress display
         * function. This function should be defined as the curl_xferinfo_callback
         * prototype defines. (Deprecates CURLOPT_PROGRESSFUNCTION) */
        XFERINFOFUNCTION = CURLOPTTYPE.FUNCTIONPOINT + 219,

        /* The XOAUTH2 bearer token */
        XOAUTH2_BEARER = CURLOPTTYPE.STRINGPOINT + 220,

        /* Set the interface string to use as outgoing network
         * interface for DNS requests.
         * Only supported by the c-ares DNS backend */
        DNS_INTERFACE = CURLOPTTYPE.STRINGPOINT + 221,

        /* Set the local IPv4 address to use for outgoing DNS requests.
         * Only supported by the c-ares DNS backend */
        DNS_LOCAL_IP4 = CURLOPTTYPE.STRINGPOINT + 222,

        /* Set the local IPv4 address to use for outgoing DNS requests.
         * Only supported by the c-ares DNS backend */
        DNS_LOCAL_IP6 = CURLOPTTYPE.STRINGPOINT + 223,

        /* Set authentication options directly */
        LOGIN_OPTIONS = CURLOPTTYPE.STRINGPOINT + 224,

        /* Enable/disable TLS NPN extension (http2 over ssl might fail without) */
        SSL_ENABLE_NPN = CURLOPTTYPE.LONG + 225,

        /* Enable/disable TLS ALPN extension (http2 over ssl might fail without) */
        SSL_ENABLE_ALPN = CURLOPTTYPE.LONG + 226,

        /* Time to wait for a response to a HTTP request containing an
         * Expect: 100-continue header before sending the data anyway. */
        EXPECT_100_TIMEOUT_MS = CURLOPTTYPE.LONG + 227,

        /* This points to a linked list of headers used for proxy requests only,
           struct curl_slist kind */
        PROXYHEADER = CURLOPTTYPE.OBJECTPOINT + 228,

        /* Pass in a bitmask of "header options" */
        HEADEROPT = CURLOPTTYPE.LONG + 229,

        /* The public key in DER form used to validate the peer public key
           this option is used only if SSL_VERIFYPEER is true */
        PINNEDPUBLICKEY = CURLOPTTYPE.STRINGPOINT + 230,

        /* Path to Unix domain socket */
        UNIX_SOCKET_PATH = CURLOPTTYPE.STRINGPOINT + 231,

        /* Set if we should verify the certificate status. */
        SSL_VERIFYSTATUS = CURLOPTTYPE.LONG + 232,

        /* Set if we should enable TLS false start. */
        SSL_FALSESTART = CURLOPTTYPE.LONG + 233,

        /* Do not squash dot-dot sequences */
        PATH_AS_IS = CURLOPTTYPE.LONG + 234,

        /* Proxy Service Name */
        PROXY_SERVICE_NAME = CURLOPTTYPE.STRINGPOINT + 235,

        /* Service Name */
        SERVICE_NAME = CURLOPTTYPE.STRINGPOINT + 236,

        /* Wait/don't wait for pipe/mutex to clarify */
        PIPEWAIT = CURLOPTTYPE.LONG + 237,

        /* Set the protocol used when curl is given a URL without a protocol */
        DEFAULT_PROTOCOL = CURLOPTTYPE.STRINGPOINT + 238,

        /* Set stream weight, 1 - 256 (default is 16) */
        STREAM_WEIGHT = CURLOPTTYPE.LONG + 239,

        /* Set stream dependency on another CURL handle */
        STREAM_DEPENDS = CURLOPTTYPE.OBJECTPOINT + 240,

        /* Set E-xclusive stream dependency on another CURL handle */
        STREAM_DEPENDS_E = CURLOPTTYPE.OBJECTPOINT + 241,

        /* Do not send any tftp option requests to the server */
        TFTP_NO_OPTIONS = CURLOPTTYPE.LONG + 242,

        /* Linked-list of host:port:connect-to-host:connect-to-port,
           overrides the URL's host:port (only for the network layer) */
        CONNECT_TO = CURLOPTTYPE.OBJECTPOINT + 243,

        /* Set TCP Fast Open */
        TCP_FASTOPEN = CURLOPTTYPE.LONG + 244,

        /* Continue to send data if the server responds early with an
         * HTTP status code >= 300 */
        KEEP_SENDING_ON_ERROR = CURLOPTTYPE.LONG + 245,

        /* The CApath or CAfile used to validate the proxy certificate
           this option is used only if PROXY_SSL_VERIFYPEER is true */
        PROXY_CAINFO = CURLOPTTYPE.STRINGPOINT + 246,

        /* The CApath directory used to validate the proxy certificate
           this option is used only if PROXY_SSL_VERIFYPEER is true */
        PROXY_CAPATH = CURLOPTTYPE.STRINGPOINT + 247,

        /* Set if we should verify the proxy in ssl handshake,
           set 1 to verify. */
        PROXY_SSL_VERIFYPEER = CURLOPTTYPE.LONG + 248,

        /* Set if we should verify the Common name from the proxy certificate in ssl
         * handshake, set 1 to check existence, 2 to ensure that it matches
         * the provided hostname. */
        PROXY_SSL_VERIFYHOST = CURLOPTTYPE.LONG + 249,

        /* What version to specifically try to use for proxy.
           See CURL_SSLVERSION defines below. */
        PROXY_SSLVERSION = CURLOPTTYPE.LONG + 250,

        /* Set a username for authenticated TLS for proxy */
        PROXY_TLSAUTH_USERNAME = CURLOPTTYPE.STRINGPOINT + 251,

        /* Set a password for authenticated TLS for proxy */
        PROXY_TLSAUTH_PASSWORD = CURLOPTTYPE.STRINGPOINT + 252,

        /* Set authentication type for authenticated TLS for proxy */
        PROXY_TLSAUTH_TYPE = CURLOPTTYPE.STRINGPOINT + 253,

        /* name of the file keeping your private SSL-certificate for proxy */
        PROXY_SSLCERT = CURLOPTTYPE.STRINGPOINT + 254,

        /* type of the file keeping your SSL-certificate ("DER", "PEM", "ENG") for
           proxy */
        PROXY_SSLCERTTYPE = CURLOPTTYPE.STRINGPOINT + 255,

        /* name of the file keeping your private SSL-key for proxy */
        PROXY_SSLKEY = CURLOPTTYPE.STRINGPOINT + 256,

        /* type of the file keeping your private SSL-key ("DER", "PEM", "ENG") for
           proxy */
        PROXY_SSLKEYTYPE = CURLOPTTYPE.STRINGPOINT + 257,

        /* password for the SSL private key for proxy */
        PROXY_KEYPASSWD = CURLOPTTYPE.STRINGPOINT + 258,

        /* Specify which SSL ciphers to use for proxy */
        PROXY_SSL_CIPHER_LIST = CURLOPTTYPE.STRINGPOINT + 259,

        /* CRL file for proxy */
        PROXY_CRLFILE = CURLOPTTYPE.STRINGPOINT + 260,

        /* Enable/disable specific SSL features with a bitmask for proxy, see
           CURLSSLOPT_* */
        PROXY_SSL_OPTIONS = CURLOPTTYPE.LONG + 261,

        /* Name of pre proxy to use. */
        PRE_PROXY = CURLOPTTYPE.STRINGPOINT + 262,

        /* The public key in DER form used to validate the proxy public key
           this option is used only if PROXY_SSL_VERIFYPEER is true */
        PROXY_PINNEDPUBLICKEY = CURLOPTTYPE.STRINGPOINT + 263,

        /* Path to an abstract Unix domain socket */
        ABSTRACT_UNIX_SOCKET = CURLOPTTYPE.STRINGPOINT + 264,

        /* Suppress proxy CONNECT response headers from user callbacks */
        SUPPRESS_CONNECT_HEADERS = CURLOPTTYPE.LONG + 265,

        CURLOPT_LASTENTRY /* the last unused */
    }

    /// <summary>
    ///     Reference: https://github.com/curl/curl/blob/master/include/curl/multi.h
    /// </summary>
    public enum CURLMoption : uint
    {
        /// <summary>
        ///     This is the socket callback function pointer.
        /// </summary>
        SOCKETFUNCTION = CURLOPTTYPE.FUNCTIONPOINT + 1,

        /// <summary>
        ///     This is the argument passed to the socket callback.
        /// </summary>
        SOCKETDATA = CURLOPTTYPE.OBJECTPOINT + 2,

        /// <summary>
        ///     Set to 1 to enable pipelining for this multi handle.
        /// </summary>
        PIPELINING = CURLOPTTYPE.LONG + 3,

        /// <summary>
        ///     This is the timer callback function pointer.
        /// </summary>
        TIMERFUNCTION = CURLOPTTYPE.FUNCTIONPOINT + 4,

        /// <summary>
        ///     This is the argument passed to the timer callback.
        /// </summary>
        TIMERDATA = CURLOPTTYPE.OBJECTPOINT + 5,

        /// <summary>
        ///     Maximum number of entries in the connection cache.
        /// </summary>
        MAXCONNECTS = CURLOPTTYPE.LONG + 6,

        /// <summary>
        ///     Maximum number of (pipelining) connections to one host.
        /// </summary>
        MAX_HOST_CONNECTIONS = CURLOPTTYPE.LONG + 7,

        /* maximum number of requests in a pipeline */
        MAX_PIPELINE_LENGTH = CURLOPTTYPE.LONG + 8,

        /// <summary>
        ///     A connection with a content-length longer than this will not be considered for pipelining.
        /// </summary>
        CONTENT_LENGTH_PENALTY_SIZE = CURLOPTTYPE.OFF_T + 9,

        /// <summary>
        ///     A connection with a chunk length longer than this will not be considered for pipelining.
        /// </summary>
        CHUNK_LENGTH_PENALTY_SIZE = CURLOPTTYPE.OFF_T + 10,

        /// <summary>
        ///     A list of site names(+port) that are blacklisted from pipelining.
        /// </summary>
        PIPELINING_SITE_BL = CURLOPTTYPE.OBJECTPOINT + 11,

        /// <summary>
        ///     A list of server types that are blacklisted from pipelining.
        /// </summary>
        PIPELINING_SERVER_BL = CURLOPTTYPE.OBJECTPOINT + 12,

        /// <summary>
        ///     Maximum number of open connections in total.
        /// </summary>
        MAX_TOTAL_CONNECTIONS = CURLOPTTYPE.LONG + 13,

        /// <summary>
        ///     This is the server push callback function pointer.
        /// </summary>
        PUSHFUNCTION = CURLOPTTYPE.FUNCTIONPOINT + 14,

        /// <summary>
        ///     This is the argument passed to the server push callback.
        /// </summary>
        PUSHDATA = CURLOPTTYPE.OBJECTPOINT + 15,

        /// <summary>
        ///     The last unused.
        /// </summary>
        CURLMOPT_LASTENTRY
    }

    public enum CURLMSG
    {
        /// <summary>
        ///     First, not used.
        /// </summary>
        NONE,

        /// <summary>
        ///     This easy handle has completed. 'result' contains the CURLcode of the transfer.
        /// </summary>
        DONE,

        /// <summary>
        ///     Last, not used.
        /// </summary>
        LAST
    }

    public enum CURLpoll
    {
        /// <summary>
        ///     Register, not interested in readiness (yet).
        /// </summary>
        NONE = 0,

        /// <summary>
        ///     Register, interested in read readiness.
        /// </summary>
        IN = 1,

        /// <summary>
        ///     Register, interested in write readiness.
        /// </summary>
        OUT = 2,

        /// <summary>
        ///     Register, interested in both read and write readiness.
        /// </summary>
        INOUT = 3,

        /// <summary>
        ///     Unregister.
        /// </summary>
        REMOVE = 4
    }

    /// <summary>
    ///     Reference: https://github.com/curl/curl/blob/master/include/curl/curl.h
    /// </summary>
    [Flags]
    public enum CURLPROTO
    {
        NONE = 0,
        HTTP = 1 << 0,
        HTTPS = 1 << 1,
        FTP = 1 << 2,
        FTPS = 1 << 3,
        SCP = 1 << 4,
        SFTP = 1 << 5,
        TELNET = 1 << 6,
        LDAP = 1 << 7,
        LDAPS = 1 << 8,
        DICT = 1 << 9,
        FILE = 1 << 10,
        TFTP = 1 << 11,
        IMAP = 1 << 12,
        IMAPS = 1 << 13,
        POP3 = 1 << 14,
        POP3S = 1 << 15,
        SMTP = 1 << 16,
        SMTPS = 1 << 17,
        RTSP = 1 << 18,
        RTMP = 1 << 19,
        RTMPT = 1 << 20,
        RTMPE = 1 << 21,
        RTMPTE = 1 << 22,
        RTMPS = 1 << 23,
        RTMPTS = 1 << 24,
        GOPHER = 1 << 25,
        SMB = 1 << 26,
        SMBS = 1 << 27,

        /// <summary>
        ///     Enable everything.
        /// </summary>
        ALL = ~0
    }

    public enum CURLUPart
    {
       URL,
       SCHEME,
       USER,
       PASSWORD,
       OPTIONS,
       HOST,
       PORT,
       PATH,
       QUERY,
       FRAGMENT,
       ZONEID /* added in 7.65.0 */
    }

    public enum CURLUcode
    {
         CURLUE_OK,
         CURLUE_BAD_HANDLE,          /* 1 */
         CURLUE_BAD_PARTPOINTER,     /* 2 */
         CURLUE_MALFORMED_INPUT,     /* 3 */
         CURLUE_BAD_PORT_NUMBER,     /* 4 */
         CURLUE_UNSUPPORTED_SCHEME,  /* 5 */
         CURLUE_URLDECODE,           /* 6 */
         CURLUE_OUT_OF_MEMORY,       /* 7 */
         CURLUE_USER_NOT_ALLOWED,    /* 8 */
         CURLUE_UNKNOWN_PART,        /* 9 */
         CURLUE_NO_SCHEME,           /* 10 */
         CURLUE_NO_USER,             /* 11 */
         CURLUE_NO_PASSWORD,         /* 12 */
         CURLUE_NO_OPTIONS,          /* 13 */
         CURLUE_NO_HOST,             /* 14 */
         CURLUE_NO_PORT,             /* 15 */
         CURLUE_NO_QUERY,            /* 16 */
         CURLUE_NO_FRAGMENT          /* 17 */
    }

    public enum CURLUflags : ulong
    {
        NONE = 0,
        CURLU_DEFAULT_PORT = (1<<0),       /* return default port number */
        CURLU_NO_DEFAULT_PORT = (1<<1),    /* act as if no port number was set,
    if the port number matches the
    default for the scheme */
        CURLU_DEFAULT_SCHEME = (1<<2),     /* return default scheme if
    missing */
        CURLU_NON_SUPPORT_SCHEME = (1<<3), /* allow non-supported scheme */
        CURLU_PATH_AS_IS = (1<<4),         /* leave dot sequences */
        CURLU_DISALLOW_USER = (1<<5),      /* no user+password allowed */
        CURLU_URLDECODE = (1<<6),       /* URL decode on get */
        CURLU_URLENCODE = (1<<7),     /* URL encode on set */
        CURLU_APPENDQUERY = (1<<8),        /* append a form style part */
        CURLU_GUESS_SCHEME  = (1<<9),      /* legacy curl-style guessing */CURLU_NO_AUTHORITY = (1<<10)      /* Allow empty authority when the
             scheme is unknown. */
    }
}
