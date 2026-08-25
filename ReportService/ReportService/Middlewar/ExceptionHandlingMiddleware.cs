using Report.Service.Exceptions;
using System.Net;
using System.Text.Json;

namespace ReportService.Middleware
{
    public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var (status, code, message) = Map(exception);

            // Anything we didn't expect stays a real 500 — log it in full, never leak it to the client.
            if (status == HttpStatusCode.InternalServerError)
            {
                logger.LogError(exception, "Unhandled exception on {Method} {Path}",
                    context.Request.Method, context.Request.Path);
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)status;

            var payload = new { status = (int)status, code, message };
            await context.Response.WriteAsync(JsonSerializer.Serialize(payload));
        }

        private static (HttpStatusCode status, string code, string message) Map(Exception exception) => exception switch
        {
            PhotoRejectedException ex => (HttpStatusCode.UnprocessableEntity, ex.Code, ex.Message),
            KeyNotFoundException ex => (HttpStatusCode.NotFound, "NOT_FOUND", ex.Message),
            UnauthorizedAccessException ex => (HttpStatusCode.Unauthorized, "UNAUTHORIZED", ex.Message),
            InvalidOperationException ex => (HttpStatusCode.BadRequest, "INVALID_OPERATION", ex.Message),
            _ => (HttpStatusCode.InternalServerError, "SERVER_ERROR", "An unexpected error occurred."),
        };
    }
}