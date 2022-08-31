using FibonacciDTO.AppExceptions;
using FibonacciDTO.Response;
using System.Net;
using System.Text.Json;

namespace FibonacciApp.GlobalExceptionHandling
{
    public class GlobalErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalErrorHandlingMiddleware(RequestDelegate next) => _next = next;

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var result = new ResponseModelBase
            {
                ErrorMessage = exception.Message
            };

            switch (exception)
            {
                case AppException appException:
                    {
                        result.StatusCode = appException.StatusCode;
                        result.Data = appException.Detailes;
                        break;
                    }
                default:
                    {
                        result.ErrorMessage = exception.Message;
                        result.StatusCode = HttpStatusCode.InternalServerError;
                        result.Data = exception.Data;
                        break;
                    }
            }

            var exceptionResult = JsonSerializer.Serialize(result);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)result.StatusCode;
            return context.Response.WriteAsync(exceptionResult);
        }
    }
}
