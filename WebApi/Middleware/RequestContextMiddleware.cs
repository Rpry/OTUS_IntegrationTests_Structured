using System;
using System.Linq;
using System.Threading.Tasks;
using BusinessLogic.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Serilog.Context;

namespace WebApi.Middleware
{
    public class RequestContextMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestContextMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            context.Request.Headers.TryGetValue(Constants.RequestCorrelationIdName, out var requestCorId);
            context.Request.Headers.TryGetValue(Constants.RequestSetCorrelationIdName, out var requestSetCorId);
            
            var requestCorrelationId = requestCorId.FirstOrDefault() ?? Guid.NewGuid().ToString();
            var requestSetCorrelationId = requestSetCorId.FirstOrDefault() ?? Guid.NewGuid().ToString();
            
            RequestContext.SetRequestCorId(requestCorrelationId);
            RequestContext.SetRequestSetCorId(requestSetCorrelationId);

            // Serilog
            using (LogContext.PushProperty(Constants.RequestCorrelationIdName, requestCorrelationId))
            using (LogContext.PushProperty(Constants.RequestSetCorrelationIdName, requestSetCorId))
            {
                await _next.Invoke(context);
            }
        }
    }

    public static class Extensions
    {
        public static IApplicationBuilder UseRequestContextMiddleware(this IApplicationBuilder builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            return builder.UseMiddleware<RequestContextMiddleware>();
        }
    }
}