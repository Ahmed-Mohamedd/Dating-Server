using Api.Errors;
using System.Net;
using System.Text.Json;

namespace Api.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _environment;
        public ExceptionMiddleware(RequestDelegate requestDelegate, ILogger<ExceptionMiddleware> logger, IHostEnvironment environment)
        {
            _next=requestDelegate;
            _logger=logger;
            _environment=environment;
        }

        public async Task InvokeAsync(HttpContext context) 
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, ex.Message);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                var res = _environment.IsDevelopment()
                    ? new ServerErrorResponse(context.Response.StatusCode, ex.Message, ex.StackTrace?.ToString())
                    : new ServerErrorResponse(context.Response.StatusCode, ex.Message, "Internal Server Error");

                var options  = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                var json = JsonSerializer.Serialize(res, options);

                await context.Response.WriteAsync(json);
            }
        }
    }
}
