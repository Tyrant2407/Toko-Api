using Microsoft.AspNetCore.Diagnostics;

namespace TokoApi.Middlewares;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;
    private readonly IHostEnvironment _env;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger, IHostEnvironment env)
    {
        _logger = logger;
        _env = env;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "Unhandled exception: {Message}", exception.Message);

        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        httpContext.Response.ContentType = "application/json";

        // Never expose stack traces in production
        object response = _env.IsDevelopment()
            ? new { status = 500, pesan = "Terjadi kesalahan internal pada server.", detail = exception.ToString() }
            : new { status = 500, pesan = "Terjadi kesalahan internal pada server.", detail = "" };

        await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);
        return true;
    }
}