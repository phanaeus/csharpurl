using System.Collections.Generic;

namespace CurlHttpModule.ConsoleApp
{
    public class HttpBinGet
    {
        public Dictionary<string, string>? args { get; set; }
        public Dictionary<string, string>? headers { get; set; }
        public string? origin { get; set; }
        public string? url { get; set;}
    }
}