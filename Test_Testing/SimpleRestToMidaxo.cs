using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Test_Testing
{
    [TestFixture]
    public class SimpleRestToMidaxo
    {

        private static readonly string BaseUrl = @"https://staging.midevxo.net";
        private static readonly string AuthUri = @"api/auth";
        private static readonly string SignInUri = AuthUri + @"/sign_in";
        private static readonly string SelfUri = AuthUri + @"/self";

        private CookieContainer cookies_;
        private HttpClient httpClient;

        [Test]
        public async Task Auth_SignInAsync()
        {
            HttpClient httpClient1 = new HttpClient()
            {
                BaseAddress = new Uri(@"https://staging.midevxo.net")
            };

            var authData = new { username = "proc6@perf_test_comp.net", password = "Passw0rd" };

            var authContent = new StringContent(JsonConvert.SerializeObject(authData), Encoding.UTF8, "application/json");

            var resp = await httpClient1.PostAsync(@"api/auth/sign_in", authContent);

            var resCode = resp.StatusCode;
            Console.WriteLine(resCode);

            var authData2 = new { username = "proc7@perf_test_comp.net", password = "Passw0rd" };

            var authContent2 = new StringContent(JsonConvert.SerializeObject(authData), Encoding.UTF8, "application/json");

            var resp2 = await httpClient1.PostAsync(@"api/auth/sign_in", authContent2);
            Console.WriteLine(resp2.StatusCode);

            var respHeaders = resp.Headers.ToString();

            var respHeaders2 = resp2.Headers.ToString();

            var _XSRF_token = ExtractXSRFcookieKey(resp.Headers);

            var _XSRF_token2 = ExtractXSRFcookieKey(resp2.Headers);

        }

        [Test]
        public async Task Test_Self()
        {
            cookies_ = new CookieContainer();
            HttpClientHandler handler = new HttpClientHandler()
            {
                CookieContainer = cookies_,
                Proxy = new WebProxy(@"http://localhost:8888", false),
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            };

            httpClient = new HttpClient(handler)
            {
                BaseAddress = new Uri(BaseUrl)
            };

            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Connection.Add("keep-alive");
            httpClient.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
            httpClient.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("deflate"));

            await MidaxoSignIn("proc6@perf_test_comp.net", "Passw0rd");
            var res = await MidaxoSelf();
            Assert.That(res.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var self_str = await res.Content.ReadAsStringAsync();

            
        }

        public async Task<HttpResponseMessage> MidaxoSignIn(string username, string password)
        {
            var authData = new { username = username, password = password };
            var authContent = new StringContent(JsonConvert.SerializeObject(authData), Encoding.UTF8, "application/json");

            var resp = await httpClient.PostAsync(SignInUri, authContent);
            resp.EnsureSuccessStatusCode();
            UpdateXSRF_Token(httpClient, resp);

            return resp;
        }

        public async Task<HttpResponseMessage> MidaxoSelf()
        {
            var resp = await httpClient.GetAsync(SelfUri);
            resp.EnsureSuccessStatusCode();
            UpdateXSRF_Token(httpClient, resp);

            return resp;
        }

        private void UpdateXSRF_Token(HttpClient client, HttpResponseMessage resp)
        {
            //var xsrftoken_ = ExtractXSRFcookieKey(resp.Headers);
            var xsrftoken_ = ExtractXSRFTokenFromCookies();
            if (!string.IsNullOrEmpty(xsrftoken_))
                client.DefaultRequestHeaders.Add("XSRF-TOKEN", xsrftoken_);
        }

        private string ExtractXSRFTokenFromCookies()
        {
            var xsrftoken_ = cookies_.GetCookies(new Uri(BaseUrl))["XSRF-TOKEN"]?.Value;
            Console.WriteLine(xsrftoken_);

            return xsrftoken_;
        }

        private string ExtractXSRFcookieKey(HttpResponseHeaders headers)
        {
            string XSRFCookieKey = "";

            var cookieHeaders = headers.Where(h => h.Key.Equals("Set-Cookie", StringComparison.CurrentCultureIgnoreCase));
            foreach (var item in cookieHeaders)
            {
                foreach (var ch in item.Value)
                {
                    if (!string.IsNullOrEmpty(XSRFCookieKey = ch.Split(new[] { ";", "\n" }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault(h => h.StartsWith("XSRF-TOKEN", StringComparison.CurrentCultureIgnoreCase))))
                        return XSRFCookieKey.Substring(XSRFCookieKey.IndexOf("=") + 1);
                }
            }

            return XSRFCookieKey;
        }
    }

    public class MidaxoHttpClient : HttpClient
    {
        private HttpClientHandler _handler;

        public MidaxoHttpClient() : this(new HttpClientHandler() { CookieContainer = new CookieContainer(),
                                                                   AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip}) { }

        public MidaxoHttpClient(HttpMessageHandler handler) : base(handler, true)
        {
            _handler = (HttpClientHandler) handler;

            DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            DefaultRequestHeaders.Connection.Add("keep-alive");
            DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
            DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("deflate"));
        }

        public CookieContainer Cookie { get => _handler.CookieContainer; }

        public override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
