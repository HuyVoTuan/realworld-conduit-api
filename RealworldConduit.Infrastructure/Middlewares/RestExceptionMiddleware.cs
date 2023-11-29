using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using RealWorldConduit.Infrastructure.Common;
using System.Net;

namespace RealWorldConduit.Infrastructure.Middlewares
{
    public class RestExceptionMiddleware
    {
        private readonly ILogger<RestExceptionMiddleware> _logger;
        private readonly RequestDelegate _next;

        public RestExceptionMiddleware(ILogger<RestExceptionMiddleware> logger, RequestDelegate next)
        {
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            httpContext.Response.ContentType = "application/json";

            try
            {
                await _next(httpContext);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, exception.Message);

                if (exception is RestException)
                {
                    httpContext.Response.StatusCode = (int)exception.Data[RestException.STATUS_CODE];
                    await httpContext.Response.WriteAsJsonAsync(new BaseResponse
                    {
                        Code = (HttpStatusCode)httpContext.Response.StatusCode,
                        Message = exception.Message
                    });
                }
                else
                {
                    httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    await httpContext.Response.WriteAsJsonAsync(new BaseResponse
                    {
                        Code = (HttpStatusCode)httpContext.Response.StatusCode,
                        Message = "An unexpected internal error occurred"
                    });
                }
            }
        }
    }
}
