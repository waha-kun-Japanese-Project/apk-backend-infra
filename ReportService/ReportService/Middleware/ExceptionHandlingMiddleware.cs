using System.Net;
using System.Text.Json;
using Grpc.Core;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Refit;

namespace ReportService.Middleware
{
    /// <summary>
    /// Turns every exception into <c>{ status, code, message, traceId }</c>. The mobile app maps
    /// <c>code</c> to Arabic copy (api/errorMessages.ts), so a code that is not in this table
    /// falls back to a bare "server error" for the farmer. Restored after commit 11f38f3 removed it:
    /// without it a refused photo, a dead vision tunnel and a storage outage were all the same 500.
    /// </summary>
    public class ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex) when (!context.Response.HasStarted)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var (status, code, message) = Map(context, exception);

            // Expected refusals (bad photo, bad input) are a warning; anything that means a
            // dependency or the server itself failed is an error with the full stack, so the
            // next 500 report can be traced from the traceId the client was handed.
            if ((int)status >= 500)
            {
                logger.LogError(exception,
                    "{Code} on {Method} {Path} (trace {TraceId})",
                    code, context.Request.Method, context.Request.Path, context.TraceIdentifier);
            }
            else
            {
                logger.LogWarning(
                    "{Code} on {Method} {Path}: {Message}",
                    code, context.Request.Method, context.Request.Path, message);
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)status;

            var payload = new { status = (int)status, code, message, traceId = context.TraceIdentifier };
            await context.Response.WriteAsync(JsonSerializer.Serialize(payload));
        }

        private static (HttpStatusCode status, string code, string message) Map(
            HttpContext context, Exception exception) => exception switch
        {
            // The client went away: not a fault, and nobody is left to read the body.
            OperationCanceledException when context.RequestAborted.IsCancellationRequested
                => ((HttpStatusCode)499, "REQUEST_CANCELLED", "The request was cancelled."),

            ArgumentNullException { ParamName: "Photo" }
                => (HttpStatusCode.BadRequest, "PHOTO_REQUIRED", "A photo is required."),

            // The vision service answered, but declined to diagnose the photo. Its own Arabic
            // reason travels in the message, which the app shows on the "unrecognised photo" screen.
            InvalidOperationException ex
                => (HttpStatusCode.BadRequest, "INVALID_OPERATION", ex.Message),

            KeyNotFoundException ex
                => (HttpStatusCode.NotFound, "NOT_FOUND", ex.Message),

            UnauthorizedAccessException ex
                => (HttpStatusCode.Unauthorized, "UNAUTHORIZED", ex.Message),

            // MediaStorageService (gRPC) is down or refused the upload.
            RpcException
                => (HttpStatusCode.ServiceUnavailable, "STORAGE_SERVICE_UNAVAILABLE",
                    "The storage service is unavailable."),

            // The vision service answered with an error status, was unreachable, or timed out
            // (the Refit client has a 60 s timeout, which surfaces as a TaskCanceledException).
            ApiException or HttpRequestException or TaskCanceledException
                => (HttpStatusCode.ServiceUnavailable, "AI_SERVICE_UNAVAILABLE",
                    "The AI diagnosis service is unavailable."),

            DbUpdateException or SqlException
                => (HttpStatusCode.ServiceUnavailable, "DATABASE_ERROR",
                    "The database is unavailable or rejected the request."),

            _ => (HttpStatusCode.InternalServerError, "SERVER_ERROR", "An unexpected error occurred."),
        };
    }
}
