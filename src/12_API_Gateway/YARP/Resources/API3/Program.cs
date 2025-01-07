var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/api3", () => "Hello from API3!");

app.Run();