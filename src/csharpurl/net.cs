using System;
using System.Linq;

namespace CSharpUrl
{
    public class BasicNetworkCredentials
    {
        private readonly string _s;
        public string Username { get; }
        public string Password { get; }

        public override string ToString() => _s;

        public BasicNetworkCredentials(in string username, in string password)
        {
            _s =$"{Username}:{Password}";
            Username = username;
            Password = password;
        }

        public static (bool success, BasicNetworkCredentials? credentials) TryParse(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return default;

            var sp = input.Split(new[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
            if (sp.Length < 2)
                return default;

            var p = string.Join("", sp.Skip(1));
            var (username, pw) = (sp[0], p);
            return (true, new BasicNetworkCredentials(username, pw));
        }
    }

    public class Proxy
    {
        public Uri Uri { get; }
        public BasicNetworkCredentials? Credentials { get; }

        public Proxy(Uri uri, BasicNetworkCredentials? credentials = default)
        {
            Uri = uri;
            Credentials = credentials;
        }

        public Proxy(string uri)
        {
            if (!Uri.TryCreate(uri, UriKind.Absolute, out var u))
                throw new ArgumentException("invalid uri", nameof(uri));
            Uri = u;
            var (succ, cred) = BasicNetworkCredentials.TryParse(u.UserInfo);
            if (succ)
                Credentials = cred;
        }

        public override string ToString()
        {
            return Uri.ToString();
        }

        public static (bool success, Proxy proxy) TryParse(string input)
        {
            if (!Uri.TryCreate(input, UriKind.Absolute, out var u)
                && !Uri.TryCreate($"http://{input}", UriKind.Absolute, out u)) {
                return default;
            }

            var (_, cred) = BasicNetworkCredentials.TryParse(u.UserInfo);
            return (true, new Proxy(u, cred));
        }
    }
}
