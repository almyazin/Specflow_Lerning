using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Midaxo.Event.Test.Client
{
    public class MidaxoHttpClient : HttpClient
    {
        #region Private members
        private HttpMessageHandler _handler;
        #endregion

        public CookieContainer Cookie => ((HttpClientHandler)_handler).CookieContainer;

        public MidaxoHttpClient() : this(new HttpClientHandler()
        {
            CookieContainer = new CookieContainer(),
            AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip
        })
        { }

        public MidaxoHttpClient(HttpMessageHandler handler) : base(handler, true)
        {
            _handler = handler;

            DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            DefaultRequestHeaders.Connection.Add("keep-alive");
            DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
            DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("deflate"));
        }
    }
}
