using JR_NK_MVC_Core.Common.Cache;
using JR_NK_MVC_Core.Common.JWT;
using JR_NK_MVC_Core.Common.Logger;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace JR_NK_MVC_Core.Common.Socket
{
    public class WebSocketFilter : ActionFilterAttribute
    {
        private ILoggerHelper _logger;
        public WebSocketFilter(ILoggerHelper logger)
        {
            _logger = logger;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            try
            {
                await next();
                //Console.WriteLine("WebSocketFilter-OnActionExecutionAsync-in");
                HttpContext httpContext = context.HttpContext;
                if (httpContext.WebSockets.IsWebSocketRequest)
                {
                    string method = httpContext.Request.Query["method"].ToString();
                    switch (method)
                    {
                        case "CreateWebSocket":
                            await WebSocketConnectionManager.OnConnectedAsync(httpContext, _logger);
                            break;
                    }
                }
                //Console.WriteLine("WebSocketFilter-OnActionExecutionAsync-end");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace.ToString());
                Console.WriteLine(e.Message);
            }
        }
    }
}
