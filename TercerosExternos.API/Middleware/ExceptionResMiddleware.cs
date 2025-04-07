using System.Net;
using System.Text.Json;
using TercerosExternos.Application.Common.Exceptions;

namespace TercerosExternos.API.Middleware
{
    public class ExceptionResMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionResMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await ExceptionAsync(context, ex);
            }
        }

        private async Task ExceptionAsync(HttpContext context, Exception ex)
        {
            var response = context.Response;
            response.ContentType = "application/json";
            string message;

            switch (ex)
            {
                case ApiException:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    message = ex.Message;
                    break;

                default:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    message = ex.Message;
                    break;
            }

            var messageException = new
            {
                message
            };

            var responseBody = JsonSerializer.Serialize(messageException);
            await response.WriteAsync(responseBody);

            return;

        }

    }
}
