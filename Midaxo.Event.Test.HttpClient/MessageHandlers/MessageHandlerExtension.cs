using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Midaxo.Event.Test.Client.MessageHandlers
{
    public static class MessageHandlerExtension
    {
        public static DelegatingHandler DecorateWith(this HttpMessageHandler decoratedHandler, DelegatingHandler handler)
        {
            handler.InnerHandler = decoratedHandler;

            return handler;
        }
    }
}
