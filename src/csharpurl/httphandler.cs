using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace CSharpUrl
{
    using Kvp = ValueTuple<string, string>;
    public class CurlHttpHandler : System.Net.Http.HttpMessageHandler
    {
        public bool AutoRedirect { get; set; } = true;
        public Proxy? Proxy { get; set; }
        public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(180.0);
        public Dictionary<string, Cookie> Cookies { get; } = new Dictionary<string, Cookie>();
        public bool KeepAlive { get; set; } = true;

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var headers = new List<Kvp>();
            foreach (var h in request.Headers)
                headers.Add((h.Key, string.Join(", ", h.Value)));
            if (request.Content != null)
            {
                foreach (var h in request.Content.Headers)
                    headers.Add((h.Key, string.Join(", ", h.Value)));
            }

            HttpContent? content = default;
            if (request.Content != null)
            {
                var reqContent = await request.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
                content = new ReadOnlyMemoryHttpContent(reqContent);
            }

            try
            {
                var req = new HttpReq(request.Method.ToString(), request.RequestUri) {
                    Headers = headers,
                    AutoRedirect = AutoRedirect,
                    Proxy = Proxy,
                    Timeout = Timeout,
                    ProtocolVersion = request.Version,
                    ContentBody = content,
                    Cookies = Cookies.Values,
                    KeepAlive = KeepAlive
                };
                using var resp = await HttpModule.RetrRespAsync(req).ConfigureAwait(false);

                var copy = new byte[resp.ContentData.Length];
                resp.ContentData.CopyTo(copy);

                var respMsg = new HttpResponseMessage((System.Net.HttpStatusCode)((int)resp.StatusCode)) {
                    RequestMessage = request,
                    Version = request.Version,
                    Content = new ByteArrayContent(copy),
                };

                foreach (var kvp in resp.Headers)
                {
                    if (Utils.InvariantEquals(kvp.Key, "content-type"))
                    {
                        MediaTypeHeaderValue.TryParse(kvp.Value.Last(), out var media);
                        respMsg.Content.Headers.ContentType = media;
                        //respMsg.Content.Headers.ContentType = new MediaTypeHeaderValue());
                    }
                    else if (Utils.InvariantEquals(kvp.Key, "content-length"))
                    {
                        respMsg.Content.Headers.ContentLength = resp.Content.Length;
                    }
                    else if (!respMsg.Headers.TryAddWithoutValidation(kvp.Key, kvp.Value))
                    {
                        if (!respMsg.Content.Headers.TryAddWithoutValidation(kvp.Key, kvp.Value) == (false))
                            Console.Error.WriteLine($"WARNING: Failed to add header in curl http handler: {kvp.Key}");
                    }
                }

                return respMsg;
            }
            catch (Exception e) when (e is InvalidOperationException || e is TimeoutException)
            {
                throw new HttpRequestException(e.Message, e);
            }
            finally { content?.Dispose(); }
        }
    }
}