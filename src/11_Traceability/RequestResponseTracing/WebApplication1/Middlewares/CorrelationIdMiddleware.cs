using Microsoft.Extensions.Primitives;

namespace WebApplication1.Middlewares;

public class CorrelationIdMiddleware(RequestDelegate next)
{ 
    const string correlationIdHeaderKey = "X-Correlation-ID";

    public async Task Invoke(HttpContext httpContext, ILogger<CorrelationIdMiddleware> logger)
    {
        string correlationId = Guid.NewGuid().ToString();

        if (httpContext.Request.Headers.TryGetValue(correlationIdHeaderKey, out StringValues _correlationId))
        {
            correlationId = _correlationId.ToString();
        }

        else
        {
            httpContext.Request.Headers.Add(correlationIdHeaderKey, correlationId);
        }
        
        NLog.MappedDiagnosticsContext.Set("CorrelationId", correlationId);
        
        logger.LogDebug("Asp.NET Core App , CorrelationId example");
        
        httpContext.Response.OnStarting(() =>
        {
            if (!httpContext.Response.Headers.TryGetValue(correlationIdHeaderKey, out _))
                httpContext.Response.Headers.Add(correlationIdHeaderKey, correlationId);

            return Task.CompletedTask;
        });
        
        httpContext.Items["CorrelationId"] = correlationId;
        
        await next(httpContext);
    }
}