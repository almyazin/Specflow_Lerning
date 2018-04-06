using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Midaxo.Event.Test.Client.MessageHandlers
{
    public class XSRFAuthMessageHandler : DelegatingHandler
    {
        private CookieContainer _cookieContainer;

        public XSRFAuthMessageHandler(CookieContainer cookieContainer)
        {
            _cookieContainer = cookieContainer;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var xsrftoken_ = _cookieContainer.GetCookies(request.RequestUri)["XSRF-TOKEN"]?.Value;
            request.Headers.Add("XSRF-TOKEN", xsrftoken_);

            return base.SendAsync(request, cancellationToken);
        }
    }
}
