using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace HelsiApi.Midleware.ExceptionHandler
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                await context.Response.WriteAsync("Something goes wrong.");
            }
        }
    }
}
