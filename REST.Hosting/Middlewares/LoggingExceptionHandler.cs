using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Serilog;

namespace REST.Hosting.Middlewares
{
    public class LoggingExceptionHandler : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
                Log.Fatal(ex.Message);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var exceptionModelResult = new ExceptionModel
            {
                HttpMethod = context.Request.Method,
                ExceptionMessage = ex.Message,
                InnerException = ex.InnerException?.Message
            };
            var json = JsonConvert.SerializeObject(exceptionModelResult);
            return context.Response.WriteAsync(json);
        }
    }

    internal class ExceptionModel
    {
        public string HttpMethod { get; set; }
        public string ExceptionMessage { get; set; }
        public string InnerException { get; set; }
    }
}