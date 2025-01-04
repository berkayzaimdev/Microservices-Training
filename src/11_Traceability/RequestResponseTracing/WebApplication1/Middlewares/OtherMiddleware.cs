namespace WebApplication1.Middlewares;

public class OtherMiddleware(RequestDelegate next)
{ 
    const string correlationIdHeaderKey = "X-Correlation-ID";

    public async Task Invoke(HttpContext httpContext, ILogger<OtherMiddleware> logger)
    {
        var correlationId = httpContext.Request.Headers[correlationIdHeaderKey].FirstOrDefault();
        
        correlationId = httpContext.Items["CorrelationId"].ToString();
        
        NLog.MappedDiagnosticsContext.Set("CorrelationId", correlationId);
        
        logger.LogDebug("Asp.NET Core App , CorrelationId example 2");

        await next(httpContext);
    }
}