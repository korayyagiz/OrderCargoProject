using System.Net;
using System.Text.Json;
using OrderAndCargo.Application.Exceptions;

namespace OrderAndCargo.API.Middlewares
{
    public class ApiExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ApiExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ApiValidationException ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Response.ContentType = "application/json";
                var response = JsonSerializer.Serialize(new { errors = ex.Errors });
                await context.Response.WriteAsync(response);
            }
        }
    }
}
