using NLog.Web;
using WebApplication1.Middlewares;

var builder = WebApplication.CreateBuilder(args);

#region NLog Setup

builder.Logging.ClearProviders();
builder.Host.UseNLog();

#endregion

var app = builder.Build();

app.UseMiddleware<CorrelationIdMiddleware>();
app.UseMiddleware<OtherMiddleware>();

app.MapGet("/", (HttpContext httpContext, ILogger<Program> logger) =>
{
    var correlationId = httpContext.Request.Headers["X-Correlation-ID"].FirstOrDefault();
        
    correlationId = httpContext.Items["CorrelationId"].ToString();
        
    NLog.MappedDiagnosticsContext.Set("CorrelationId", correlationId);
        
    logger.LogDebug("MinimalApi Log");
});



app.Run();