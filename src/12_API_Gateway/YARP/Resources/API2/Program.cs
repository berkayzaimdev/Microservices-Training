var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/api2", () => "Hello from API2!");

app.Run();