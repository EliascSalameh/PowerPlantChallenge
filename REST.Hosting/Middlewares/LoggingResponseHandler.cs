using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace REST.Hosting.Middlewares
{
    public class LoggingResponseHandler : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var originalBodyStream = context.Response.Body;
            try
            {
                using var ms = new MemoryStream();
                context.Response.Body = ms;
                await next(context);
                context.Response.Body.Position = 0;
                var responseReader = new StreamReader(context.Response.Body);
                var responseContent = await responseReader.ReadToEndAsync();
                Log.Information(responseContent);
                ms.Seek(0, SeekOrigin.Begin);
                await ms.CopyToAsync(originalBodyStream);
            }
            finally
            {
                context.Response.Body = originalBodyStream;
            }
        }
    }
}