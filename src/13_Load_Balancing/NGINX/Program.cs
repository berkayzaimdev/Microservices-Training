var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.MapGet("/", (HttpContext context) =>
{
    var client_ip = context.Request.Headers["X-Forwarded-For"].ToString();

    return args[0] + " - " + client_ip;
});

app.Run();

/*
    * Algoritmaların Kullanımı
       * Default -> round robin kullanılır. Sunucuların eşit güce sahip olduğu senaryolara uygundur
       * Least Connection -> least_conn; ana parametresi ile sağlanır. Gelen isteği en az sayıda etkin bağlantıya sahip sunucuya yönlendirir
       * IP Hash -> ip_hash; ana parametresi ile sağlanır. IP adresi bazlı yönlendirme sağlar
       * Weighted Round Robin -> sunucuya verilen weight=n; parametresi ile sağlanır. 2 sunucu için; gelen n+1 isteğin n'i ağırlık verilen sunucuya, 1'i ise defaulta yönlendirilir
       * Generic Hash -> hash $param consistent; parametresi ile sağlanır. Parametre bazlı yönlendirme
       * Least Time -> gelen isteği en düşük ortalama gecikme değerine sahip sunucuya yönlendirir. least_time ana parametresi ile sağlanır
          * header -> sunucudan gelen ilk byte'ın geliş zamanına göre yönlendirme
          * last_byte -> sunucudan tam yanıt alma zamanına göre yönlendirme
          * last_byte inflight -> Eksik istekleri de dikkate alarak last_byte yaklaşımını uygular
          * random n -> rastgele n sunucu dikkate alınarak uygulanan yaklaşım
 
    * Server Weights
       * Load balancing uygularken trafiği sunucuların gücüne veya kapasitesine göre ayarlamamızı sağlar
       * server backup -> işaretli sunucu diğer sunucular pasifse istek alamayacaktır


    * Slow Start
       * slow_start=t; Yakın zamanda onarılan bir sunucu t süre boyunca trafik kademeli olarak artırılır.


    * Enabling Session Persistence
       * sticky cookie -> gelen ilk isteğe bir oturum tanımlama bilgisi ekler
       * sticky route  -> gelen ilk istek alındığında client'a bir rota verir ve daha sonra bu rota baz alınarak eşleşen sunucuya tekrar istek atılır
       * sticky learn  -> gelen istekler neticesinde yanıtları inceleyerek hangi sunucuya yönlendirme yapılacağını öğrenen yaklaşım

    * İsteği yapan client'ın IP değerine erişmek
       * X-Forwarded-For key sayesinde header'da client'a ait IP taşınır
*/