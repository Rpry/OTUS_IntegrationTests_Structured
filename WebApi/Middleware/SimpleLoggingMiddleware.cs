using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace WebApi.Middleware
{
    public class SimpleLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        private const string MessageTemplate = "RequestType: {RequestType}, RequestPath: {RequestPath}, ResponseCode: {ResponseCode}, Elapsed: {Elapsed}";

        public SimpleLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext, ILogger<SimpleLoggingMiddleware> logger)
        {
            var timer = new Stopwatch();
            timer.Start();
            await _next(httpContext);
            timer.Stop();
            var elapsedMs = timer.Elapsed;

            logger.Log(LogLevel.Information, MessageTemplate,
                httpContext.Request.Method, httpContext.Request.Path,
                httpContext.Response?.StatusCode, elapsedMs.TotalMilliseconds
            );
        }
    }

    public static class LoggingMiddlewareAppExtensions
    {
        public static IApplicationBuilder UseSimpleHttpLogging(this IApplicationBuilder applicationBuilder)
        {
            return applicationBuilder.UseMiddleware<SimpleLoggingMiddleware>();
        }
    }
}