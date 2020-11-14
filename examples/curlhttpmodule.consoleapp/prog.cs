using CSharpUrl;
using CSharpUrl.LibCurl;
using System;
using System.Buffers;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;


namespace Oopz.ConsoleApp
{
    using StrKvp = ValueTuple<string, string>;

    internal static class Program
    {
        private static ReadOnlyCollection<StrKvp> Headers()
        {
            var headers = new List<StrKvp>(3) {
                ("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36"),
                ("Accept-Encoding", "gzip, deflate, br"),
                ("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8"),
            };
            return new ReadOnlyCollection<StrKvp>(headers);
        }

        private static Dictionary<string, Cookie> Cookies()
        {
            var cookies = new Dictionary<string, Cookie> {
                //["name0"] = new Cookie("name0", "value0")
            };
            return new Dictionary<string, Cookie>(cookies);
        }

        private static Proxy? Proxy()
        {
            //return default;
            return new Proxy("socks5h://localhost:8889/");
        }

        private static HttpReq Req(
            string method,
            string uri,
#nullable enable
            HttpContent? content = null)
#nullable disable
        {
           
            var req = new HttpReq(method, uri) {
                Cookies = Cookies().Values,
                Headers = Headers(),
                Proxy = Proxy(),
                Insecure = true,
                ContentBody = content,
                ProtocolVersion = HttpVersion.Http2,
                //Timeout = TimeSpan.FromSeconds(99.0)
            };
            return req;
        }


        private static async Task GZip()
        {
            var req = Req("GET", "https://nghttp2.org/httpbin/gzip");
            using var resp = await HttpModule.RetrRespAsync(req).ConfigureAwait(false);
            Debug.Assert(resp.StatusCode == HttpStatusCode.OK);
        }

        private static async Task Brotli()
        {
            var req = Req("GET", "https://nghttp2.org/httpbin/brotli");
            using var resp = await HttpModule.RetrRespAsync(req).ConfigureAwait(false);
            Debug.Assert(resp.StatusCode == HttpStatusCode.OK);
        }

        private static async Task IpLeak()
        {
            var req = Req("GET", "https://www.dnsleaktest.com/");
            using var resp = await HttpModule.RetrRespAsync(req).ConfigureAwait(false);
            Console.WriteLine(resp);
        }

        private static async Task Get()
        {
            var req = Req("GET", "https://httpbin.org/get?hello=2&two=two");
            using var resp = await HttpModule.RetrRespAsync(req).ConfigureAwait(false);
            
            //var httpGet = JsonSerializer.Deserialize<HttpBinGet>(resp.ContentData.Span.Slice(0, resp.ContentData.Length - 1));
        }

        private static async Task Post()
        {
            var sequence = new List<StrKvp>(2) {
                ("Accept", "value0")
            };
            using var content = new EncodedFormValuesHttpContent(sequence);
            var req = Req("POST", "https://nghttp2.org/httpbin/post", content);
            req.Headers = sequence;
            using var cts = new CancellationTokenSource(5000);
            using var resp = await HttpModule.RetrRespAsync(req, cts.Token).ConfigureAwait(false);
            Debug.Assert(resp.StatusCode == HttpStatusCode.OK);
        }



        private static async Task SetCookies()
        {
            var req = Req("GET", "https://nghttp2.org/httpbin/cookies/set?name5=value5&name6=value6");
            using var resp = await HttpModule.RetrRespAsync(req).ConfigureAwait(false);
            Debug.Assert(resp.StatusCode == HttpStatusCode.OK);
            Debug.Assert(resp.Cookies.Count == 2);
        }

        private static async Task AutoRedirect()
        {
            var req = Req("GET", "https://nghttp2.org/httpbin/absolute-redirect/5");
            using var resp = await HttpModule.RetrRespAsync(req).ConfigureAwait(false);
            Debug.Assert(resp.StatusCode == HttpStatusCode.OK);
        }

        private static async Task AutoRedirectDisabled()
        {
            var req = Req("GET", "https://nghttp2.org/httpbin/absolute-redirect/5");
            req.AutoRedirect = false;
            using var resp = await HttpModule.RetrRespAsync(req).ConfigureAwait(false);
            Debug.Assert(resp.StatusCode == HttpStatusCode.Found);
        }

        // thanks to https://ahrefs.com/blog/most-visited-websites/
        private static Queue<string> _uris = new Queue<string>(new List<string> {
            "https://httpbin.org/get",
            "https://duckduckgo.com/",
            "https://www.startpage.com/"
            //"https://youtube.com", // 1 1,705,778,109
            //"https://en.wikipedia.org", // 2 1,229,282,645
            //"https://facebook.com", // 3 616,445,886
            //"https://twitter.com", // 4 573,405,781
            //"https://amazon.com", // 5 533,982,248
            //"https://imdb.com", // 6 211,647,294
            //"https://reddit.com", // 7 187,130,346
            //"https://pinterest.com", // 8 157,685,591
            //"https://ebay.com", // 9 115,483,384
            //"https://tripadvisor.com", // 10 114,788,186
            ////"https://craigslist.org", // 11 104,200,537
            //"https://walmart.com", // 12 101,055,814
            //"https://instagram.com", // 13 96,693,255
            //"https://google.com", // 14 94,713,345
            //"https://nytimes.com", // 15 89,163,505
            //"https://apple.com", // 16 79,520,489
            //"https://linkedin.com", // 17 78,039,964
            //"https://indeed.com", // 18 72,641,193
            //"https://play.google.com", // 19 69,157,719
            //"https://espn.com", // 20 64,536,957
            //"https://webmd.com", // 21 57,614,599
            //"https://cnn.com", // 22 56,948,268
            //"https://homedepot.com", // 23 55,178,848
            //"https://etsy.com", // 24 54,256,074
            //"https://netflix.com", // 25 52,321,111
            //"https://quora.com", // 26 51,029,162
            //"https://microsoft.com", // 27 49,182,997
            //"https://target.com", // 28 49,101,029
            //"https://merriam-webster.com", // 29 46,709,618
            //"https://forbes.com", // 30 45,258,742
            //"https://mapquest.com", // 31 45,167,474
            //// "https://nih.gov", // 32 43,549,321
            //"https://gamepedia.com", // 33 42,257,632
            //"https://yahoo.com", // 34 42,158,719
            //"https://healthline.com", // 35 41,530,169
            //"https://foxnews.com", // 36 41,176,256
            //// "https://allrecipes.com", // 37 40,844,933
            //// "https://quizlet.com", // 38 40,618,283
            //"https://weather.com", // 39 40,260,302
            //// "https://bestbuy.com", // 40 39,074,232
            //"https://urbandictionary.com", // 41 39,017,162
            //"https://mayoclinic.org", // 42 38,992,026
            //"https://aol.com", // 43 37,399,458
            //"https://genius.com", // 44 37,074,935
            //"https://zillow.com", // 45 37,058,474
            //"https://usatoday.com", // 46 34,572,355
            //"https://glassdoor.com", // 47 34,491,698
            //"https://msn.com", // 48 34,242,923
            //"https://rottentomatoes.com", // 49 33,880,159
            //"https://lowes.com", // 50 33,541,360
            //"https://dictionary.com", // 51 33,538,804
            //"https://businessinsider.com", // 52 33,133,115
            //// "https://usnews.com", // 53 33,018,191
            //"https://medicalnewstoday.com", // 54 31,948,351
            //"https://britannica.com", // 55 31,881,909
            //"https://washingtonpost.com", // 56 31,773,083
            //"https://usps.com", // 57 31,669,446
            //"https://finance.yahoo.com", // 58 29,223,585
            //// "https://irs.gov", // 59 28,159,399 fuck the irs
            //"https://yellowpages.com", // 60 26,776,664
            //// "https://chase.com", // 61 26,382,428
            //// "https://retailmenot.com", // 62 26,310,101
            // // "https://accuweather.com", // 63 26,231,739
            //"https://wayfair.com", // 64 25,982,058
            //"https://duckduckgo.com", // 65 25,880,125
            //"https://live.com", // 66 25,781,379
            //"https://login.yahoo.com", // 67 25,314,015
            //"https://steamcommunity.com", // 68 24,989,372
            //"https://xfinity.com", // 69 24,775,935
            //"https://cnet.com", // 70 24,528,112
            //"https://ign.com", // 71 24,309,366
            //"https://steampowered.com", // 72 24,286,968
            //// "https://macys.com", // 73 23,905,144
            //"https://wikihow.com", // 74 23,663,557
            //"https://mail.yahoo.com", // 75 23,624,191
            //"https://wiktionary.org", // 76 23,349,883
            //"https://cbssports.com", // 77 23,108,488
            //// "https://cnbc.com", // 78 21,969,609
            //"https://bankofamerica.com", // 79 21,846,224
            //// "https://expedia.com", // 80 21,660,658
            //"https://wellsfargo.com", // 81 21,414,290
            //"https://groupon.com", // 82 21,100,526
            //"https://twitch.tv", // 83 20,745,459
            //"https://khanacademy.org", // 84 20,622,672
            //"https://theguardian.com", // 85 20,567,821
            //"https://paypal.com", // 86 20,473,119
            //"https://spotify.com", // 87 20,371,593
            //"https://att.com", // 88 20,023,731
            //"https://nfl.com", // 89 19,772,392
            //"https://realtor.com", // 90 19,534,146
            //// "https://ca.gov", // 91 19,484,068
            //"https://goodreads.com", // 92 19,386,754
            //"https://office.com", // 93 19,333,140
            //"https://ufl.edu", // 94 19,163,321
            //// "https://mlb.com", // 95 19,029,754
            //"https://foodnetwork.com", // 96 18,735,008
            //"https://bbc.com", // 97 18,708,731
            //"https://apartments.com", // 98 18,607,273
            //"https://npr.org", // 99 18,283,034
            //"https://wowhead.com" //100, 18,169,056
            }
        );

        private static string Uri()
        {
            lock (_uris) {
                var uri = _uris.Dequeue();
                _uris.Enqueue(uri);
                return uri;
            }
        }

#nullable enable
        private static async Task<HttpResp?> RetrResp(HttpReq req, CancellationToken c) {
            try {
                var resp =  await HttpModule.RetrRespAsync(req, c).ConfigureAwait(false);
                return resp;
            }
            catch (Exception e) when (e is InvalidOperationException || e is OperationCanceledException) {
                Console.Error.WriteLine($"http error for {req}: {e.GetType().Name} ~ {e.Message}");
                return default;
            }
        }

        
        
        private static async Task ReqMultiConcurrently(int cnt)
        {
            var responses = new List<Task<HttpResp?>>(cnt);
            using var cts = new CancellationTokenSource(120000);
            for (var i = 0; i < cnt; i++)
            {
                var req = Req("GET", Uri());
                //req.Timeout = TimeSpan.FromSeconds(300.0);
                
                responses.Add(RetrResp(req, cts.Token));
            }
            
            var results = (await Task.WhenAll(responses)).Where(value => value != null);
            foreach (var r in results)
                r.Dispose();
        }

        private static readonly Random Rando = new Random();
        private static int Random(int min, int max)
        {
            lock (Rando)
                return Rando.Next(min, max);
        }

        private static async Task RetrResponses(int index, int howManyReq)
        {
            while (howManyReq-- > 0) {
                
                var req = Req("GET", Uri());
                var resp = await RetrResp(req, CancellationToken.None).ConfigureAwait(false);
                if (resp != null) {
                    using var __ = resp;
                    Console.WriteLine($"loop.[{index}] req={req} resp={resp.StatusCode} {howManyReq}");
                    await Task.Delay(Random(50, 100)).ConfigureAwait(false);
                }
            }
        }


        private static async Task Agent(int cnt, int howManyReq)
        {
            Console.Out.WriteLine($"Executing {howManyReq} requests using {cnt} loops.");
            var tasks = new List<Task>();
            for (var i = 0; i < cnt; i++) {
                tasks.Add(RetrResponses(i, howManyReq));
            }
            await Task.WhenAll(tasks).ConfigureAwait(false);
        }

        private static async Task Multi()
        {
            var st = new Stopwatch();
            st.Start();
            Console.Out.WriteLine("awaiting 7 requests: ...");
            await ReqMultiConcurrently(7).ConfigureAwait(false);

            st.Stop();
            Console.Out.WriteLine($"awaiting 255 requests: {st.Elapsed} time elapsed.");
        }

        private static async Task MainConcurrent()
        {
            Console.Out.WriteLine("Enqueing requests: ...");
            var st = new Stopwatch();
            st.Start();
            var list = new List<Task>
            {
                Post(), GZip(), Brotli(), SetCookies(), AutoRedirect(), AutoRedirectDisabled()
            };
            await Task.WhenAll(list).ConfigureAwait(false);
            Console.Out.WriteLine($"Enqueing requests: {st.Elapsed} time elapsed.");
        }

        private static async Task MainSeq()
        {
            var st = new Stopwatch();
            st.Start();
            
            //await HttpC().ConfigureAwait(false);
            Console.Out.WriteLine("Running requests sequentially: ...");
            await Post().ConfigureAwait(false);
            
            await GZip().ConfigureAwait(false);
            await Brotli().ConfigureAwait(false);
            await SetCookies().ConfigureAwait(false);
            await AutoRedirect().ConfigureAwait(false);
            await AutoRedirectDisabled().ConfigureAwait(false);
            
            Console.Out.WriteLine($"Running requests sequentially: {st.Elapsed} time elapsed.");
        }


        private static async Task<int> MainAsync()
        {
            
            while (true) try {
                await MainSeq().ConfigureAwait(false);
                await Multi().ConfigureAwait(false);

                //await MainConcurrent().ConfigureAwait(false);
                //await Multi().ConfigureAwait(false);
                await Agent(6, 9).ConfigureAwait(false);
                // await MainSeq().ConfigureAwait(false);
                // await MainConcurrent().ConfigureAwait(false);
                // await Multi().ConfigureAwait(false);
                
                Console.Out.WriteLine("private bytes: {0}", Process.GetCurrentProcess().PrivateMemorySize64);
                Console.Out.WriteLine("press return to continue loop.");
                Console.In.ReadLine();
            }
            catch (Exception e) {
                Console.Error.WriteLine($"Error occored: {e}");
                return 1;
            }    
        

            return 0;
        }

        public static (bool success, ArraySegment<T> arrSeg) TryGetArraySegment<T>(this in ReadOnlyMemory<T> memory) =>
            MemoryMarshal.TryGetArray(memory, out var segment) ? (true, segment) : default!;

        public static ArraySegment<T> AsArraySeg<T>(this in ReadOnlyMemory<T> memory)
        {
            var (success, seg) = TryGetArraySegment(memory);
            if (!success)
                throw new InvalidOperationException("memory did not contain an array.");
            return seg;
        }

        public static Memory<byte> GZipCompress(in ReadOnlySpan<byte> input)
        {
            using var memOwner = MemoryPool<byte>.Shared.Rent(input.Length);
            input.CopyTo(memOwner.Memory.Span);
            ReadOnlyMemory<byte> buffer = memOwner.Memory.Slice(0, input.Length);
            using var output = new MemoryStream();
            var arraySeg = buffer.AsArraySeg();
            var gzip = new GZipStream(output, CompressionMode.Compress);
            try { gzip.Write(arraySeg.Array, arraySeg.Offset, arraySeg.Count); }
            finally { gzip.Dispose(); }
            return output.ToArray();
        }

        private static int Main(string[] args)
        {
            var v = libcurl.curl_version();
            Console.WriteLine(v);
            return MainAsync().GetAwaiter().GetResult();
        }
    }
}
