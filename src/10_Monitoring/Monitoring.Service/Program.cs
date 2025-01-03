using System.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHealthChecksUI(
    settings =>
    {
        settings.ConfigureApiEndpointHttpclient((serviceProvider, httpClient) =>
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "jwt1");
        });
        
        settings.ConfigureWebhooksEndpointHttpclient((serviceProvider, httpClient) =>
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "jwt2");
        });
    }
    // settings =>
    // {
    //     settings.AddHealthCheckEndpoint("Service A", "http://localhost:5207");
    //     settings.AddHealthCheckEndpoint("Service B", "http://localhost:5097");
    //     settings.SetApiMaxActiveRequests(3);     // aynı anda en fazla 3 istek gönderilebilir
    //     settings.SetEvaluationTimeInSeconds(3);  // 3 saniyelik aralıklarla kontrol edelim
    // }
    // metot boş bırakılırsa appsettings'den okuma sağlanır
).AddSqlServerStorage("Server=(localdb)\\MSSQLLocalDB;Database=HealthChecksUIDB");

/*
 * Tablolar
   * Configurations: UI'ın tükettiği servislerin bilgisini tutar
   * Executions: Servislerin hangi tarihten itibaren ve en son ne zaman sağlık durumlarının kontrol edildiğini tutar
   * Failures: Sağlık durumu unhealthy olan servisleri tutar. IsUpAndRunning = bit değer alır ve güncel çalışma durumunu ifade eder
   * HealthCheckExecutionEntries: Servisler içerisinde sağlık kontrolü yapılan sunucuları ve bilgileri tutan tablodur 
   * HealthCheckExecutionHistories: Servisler içerisinde sağlık kontrolü yapılan sunuculardaki olası hataların kaydını tutar 
*/

var app = builder.Build();

app.UseHealthChecksUI(opts => opts.UIPath = "/health-ui");

app.Run();