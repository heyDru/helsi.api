using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;

namespace HelsiApi.Midleware.ExceptionHandler
{
    public static class CustomExceptionsMiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomExceptionsMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlerMiddleware>();
        }
    }
}