using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddHealthChecks() // bir http isteği atılır; .NET'e ait middleware zincirinden geçmeden direkt olarak bir istek gönderilir ve yanıt alınır. Bu sayede, ayakta olan uygulamadan hızlı bir sağlık durumu bilgisi alınabilir
    .AddRedis(
        redisConnectionString: "localhost:6379",
        name: "Redis Check",
        failureStatus: HealthStatus.Degraded | HealthStatus.Unhealthy,
        tags: ["redis"]
    )
    //.AddMongoDb("mongodb://localhost:27017")
    .AddNpgSql(
        connectionString: "User ID=postgres;Password=123456;Host=localhost;Port=5432;Database=postgres;",
        healthQuery: "SELECT 1;",
        name: "PostgresSQL Check",
        failureStatus: HealthStatus.Degraded | HealthStatus.Unhealthy,
        tags: ["db", "sql", "postgressql"]
    );

var app = builder.Build();

app.UseHealthChecks("/health", new HealthCheckOptions()
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse // response u daha detaylı bir şekilde inceleyebiliyoruz
});

// tek bir string response döner

// Healthy
// Degraded => yazılımda bir problem yok , fakat response süresi uzun
// Unhealthy

app.Run();