# Mikroservisler

## Temel Kavramlar

- Servis: Mikroservis mimarisinin temel yapı taşıdır. Spesifik bir sorumluluğa sahiptir.
- İşlev: Her bir servisin sorumluluğunun ta kendisidir.
- Bağımsızlık: Bir servisin çökmesi yahut güncellenmesi, diğer servisleri etkilemez. Bu sayede bakım kolaylığı, dağıtım ve geliştirme gibi açılardan önemli avantajlar elde edilir.
- API: Servislerin birbirleriyle ya da client'la iletişim kurmasını sağlayan senkron arayüzlerdir.
- Message Broker: Servislerin birbirleriyle asenkron iletişim kurmasını sağlayan bir modeldir.
- Ölçeklenebilirlik: Her servisin ihtiyacına göre diğer servislerden ayrı olarak ölçeklendirilebilmesi.
- Yeniden Kullanılabilirlik: Her bir servis tek bir işleve bağımsız olarak odaklı olduğundan, farklı projelerde veya platformlarda yeniden kullanılabilir.

---

## Organizasyon Modelleri

### Teknoloji Odaklı Model
- Her bir servis, farklı bir teknoloji yığını ve dilinde geliştirilir.
- Bu sayede bir dilin en önemli gözüken avantajlarından istifade edilebilir. Farklı servislere en uygun olan dil kullanılabilir.
- Bu modelin dezavantajı ise; ekiplerin kullanılan farklı teknolojilerin her birine hakim olmasının gerekliliğidir.

### İş Odaklı Model
- Her bir servis, belirli bir iş fonksiyonuna odaklanarak geliştirilir. Örn; kullanıcı yönetimi, ödeme işlemleri ve e-ticaret ürün yönetimi gibi farklı işlevler için farklı servisler geliştirilir.
- Bu yaklaşım, ekiplerin sorumluluklarını net olarak tanımlamak konusunda faydalıdır.
- Ancak, bu modelde farklı servisler arasındaki veri paylaşımının tasarlanması ve yönetimi biraz zahmetli olabilir.

### Veri Odaklı Model
- Her bir servis, belirli bir veri grubuna odaklanarak geliştirilir. Örn; müşteri verileri, ürün kataloğu gibi veri grupları için ayrı servisler geliştirilebilir.
- Servisler arasındaki veri bütünlüğünü ve veri işleme performansını artırabilen bir yaklaşımdır.
- Bu yaklaşımda servisler arası sınırlar net olarak çizilemeyebilir, bu sebeple servisler arası bağımlılığın artmasına ve veri güncellemelerinin koordinasyonunun zorlaşmasına yol açabilir.

### Karışık Model

---

## Servisler Arası Veri İletişimi

### HTTP Tabanlı API'ler
- JSON ya da XML gibi formatlarla, senkron bir iletişim sağlar.

### Message Broker
- Asenkron bir iletişim sağlar. Servisler arası gevşek bağlılık sağladığı için önemli bir yaklaşımdır.

### RPC (Remote Procedure Call)
- Servislerin doğrudan birbirini çağırdığı bir iletişim modelidir.

---

## API Gateway

- Client'lara servislerin erişimi için merkezi bir köprü sağlar. ,
- Bu sayede servis bilgilerine gereksinim duymaksızın servislere erişebiliriz.
- Load Balancing operasyonunu destekler.
- Mikroservis mimarisinde elzem olmasa da önemli bir bileşendir.

---

## Tasarım

### Gereksinim Analizi
- İlk adım olarak projenin gereksinimlerinin net olarak anlaşılması gerekmektedir.
- Projenin özelliklerinin, fonksiyonlarının, performans hedeflerinin kısacası amacının ne olduğu belirlenmeli ve bu parametrelere göre bir rota çizilmelidir.
- Diğer ekip üyeleri, proje sahipleri ve kullanıcılarla etkileşimde bulunmak ve gereksinimlerin doğru bir şekilde anlaşıldığından emin olunmalıdır.

### Mikroservislerin Belirlenmesi
- Gereksinim analizinin ardından projenin uygun koşullar çerçevesinde servislere bölünmesi gerekir.

### İletişim Protokollerinin Belirlenmesi
- Servisler arası iletişim ve veri paylaşım protokollerinin seçimi büyük bir önem arz eder.
- O anki senaryoya uygun olarak ilgili protokoller belirlenmelidir.

### Veri Tabanı Stratejisi
- İletişim protokollerinin yanında her bir servis için gerekli ve uygun bir veri tabanı stratejisi yürütülmellidir.

### Güvenlik Tasarımı
- Mimarinin bütününü güvenli bir şekilde korumak gerekir. Bunu sağlamak için doğrulama ve yetkilendirme, veri ve ağ güvenliği gibi konular ayrıntılı bir şekilde düşünülüp uygulanmalıdır.

### Hata Yönetimi ve İzleme
- Mikroservis mimarisi, her bir serviste meydana gelebilecek olası hatalara karşı dinamik bir izlenebilirliğe sahip olmalıdır. 

---

## Bir Servisin Sınırlarını Belirlemek

### Tek Sorumululuk Prensibi
- Bir sorumluluk ele alınır, bu sorumluluk bir servise atanır ve bu sınırlara bağlı kalınır

### Dış Dünya Etkileşimi
- Servisin dış dünya ile nasıl iletişime geçeceğinin belirlenmesi işimizi kolaylaştıracaktır.

### Ekip ve Organizasyon Yapısı
- Bir ekibin niteliği ve almış oldukları sorumluluk, geliştirecekleri servisin sınırlılıklarıyla paralellik arz edebilir.

### Bağımlılıklar ve Sınırlar
- Yapılacak bir işin, farklı bir işe olan bağımlılığı servisler arasındaki sınırları çizme konusunda faydalı olabilir.

### Veri Yapısı ve Sınırı
- İşlenecek olan verinin yapısı ve sınırı, o veriyi işleyecek olan servisin sınırları ile doğru orantılı olabilir.

---

## Servislerin Kendi Aralarında Veri Paylaşımı

### API Request

### Event-Driven Architecture

### Ortak Veri Depolama
- Bazı durumlarda farklı servislerin aynı veriye ihtiyacı olabilir. Bu durumda veriyi ortak bir veritabanında tutmak mantıklı br yaklaşım olacaktır.

### API Gateway

---

## CAP Teoremi

- Dağıtık sistemlerdeki veri konusunda yaşanan zorlukları açıklamaya çalışan bir teoremdir.
- Teoride, bu kısaltmayı oluşturan 3 özelliğin aynı anda sağlanamayacağı ifade edilmektedir.
- Bu 3 özellik arasında kararlı bir denge kurulmalıdır. 
- Teoreme göre en fazla 2 özellik tam olarak sağlanabilir. Hangi özellikler önemliyse oraya yoğunlaşılır.

### Consistency Tutarlılık
- Bir veri değişiminde, yapılan bu değişiklik EN HIZLI ŞEKİLDE tüm servislerde tutarlı olmalıdır.

### Availability Erişilebilirlik
- Servislerin birinde meydana gelen problem, tüm servisi etkilememelidir. Servis erişilebilir ve yanıt verebilir durumda olmalıdır.

### Partition Tolerance Bölüm Toleransı
- Servisler arasında bir bağ varsa, o bağ kopsa dahi servisler ayrı ayrı kullanılabilir olmalıdır, bir tolerans gösterilmelidir. 

---

## Eventual Consistency

- Dağıtık sistemlerde veri tutarlılığını ve senkronizasyonunu sağlamak için kullandığımız yöntemlerden biridir.
- Verinin tüm servislerde tutarlı hale gelmesi, bir sürece bağlanmalıdır. En kısa sürede senkronizasyon sağlanmalıdır. (Arada kayıp olan süre, feda edilebilen bir tutarlılık olarak nitelendirilebilir.)
- Genel olarak, dağıtık sistemler için best-practice olarak kabul edilir.

## Strong Consistency

- *Eventual Consistency*'de olduğu gibi feda edilen mikro bir sürenin tanınmadığı yaklaşımdır.
- Tutarlılık her zaman ve anında sağlanmalıdır.
- Bu yaklaşım, getirdiği tutarlılık garantisinin yanında yüksek maliyet ve performansta kısmi düşüşler de oluşturabilir.

---

## Two-Phase Commit(2PC) Protocol

- Strong Consistency yaklaşımını uyguladığımız bir protokoldür.
- Tüm kaynaklarda, yaptığımız bir işlem, ya hepsinde tamamlanmalı ya da hepsinde iptal edilmelidir. Ortası yoktur.
- Prepare Phase ve Commit Phase olmak üzere iki aşamada gerçekleştirilir.

### Prepare Phase
1. **Koordinatör**, kullanıcıdan talebi alır.
1. Koordinatör, aldığı talep doğrultusunda işlemi ilgilendirecek olan tüm **node**'lara hazır olup olmadıklarına dair bir mesaj gönderir ve tüm **katılımcı**lardan bu mesaja dair dönüş bekler.
1. Katılımcıların hepsinden onay geldiği takdirde, **koordinatör** ikinci aşamayı başlatır. Onay gelmediği takdirde ikinci aşamaya geçilmeksizin bu talep iptal edilir.

### Commit Phase
1. Koordinatör, tüm **servis**lerin sorumluluklarının gerektirdiği operasyonları başlatması adına bu servislere **commit mesajı** gönderir.
1. Koordinatör, **katılımcı**ların işlemlerini tamamlaması üzerine yanıt bekler.
1. Katılımcıların hepsinden yanıt geldiği takdirde, kullanıcıdan gelen talep başarıyla tamamlanmış olur. Herhangi bir katılımcıdan yanıt gelmediğinde ise, işlem iptal edilip tüm servislerin yaptıklarının geri alınması için bu servislere **abort mesajı** gönderilir.

---

## Saga Pattern

- Eventual Consistency yaklaşımını uyguladığımız, bir distributed transaction yönetimi desenidir. 
- Atomik ve bütünsel bir davranış yerine, bölünmüş bir davranış sergiler. Her sistemi kendi operasyonunu yürütmesi için serbest bırakır, bir hata oluşmazsa bu adım devam eder.
- Şayet hata oluşursa, **Compensable Transaction** denen bir tür hata işleme stratejisi devreye girecek ve tüm işlemler rollback edilecektir.
- Events/Choreography ve Command/Orchestration olmak üzere iki farklı implementasyonla uygulanabilmektedir.

### Events/Choreography implementasyonu
- Servislerin, merkezi bir kontrol noktası olmaksızın birbirleriyle **event**'lar yardımıyla haberleşebilmesi prensibine dayanır. Uygulanmasında genellikle *Message Broker* yapısından istifade edilir.
- Her bir servis operasyon süresince kendi sürecine bağlı bir şekilde başarılı ya da başarısız karar vermekte ve bu neticeye göre ya kendisinden sonraki transaction'ın başlamasını sağlamakta, ya da tüm transaction'ları geri alabilmektedir. Her bir servis, **bizzat karar verici konumdadır.**
- Bu yaklaşım, distributed transaction'a katılacak olan servis sayısının 2 ile 4 arasında olduğu durumlarda tercih edilmektedir.
- Örneğin; 
	1. Order Service'ta alınan POST isteği neticesinde sipariş oluşturulur.
	1. *Order Events channel* kuyruğuna, event gönderilir.
	1. Customer Service'ta, bu event consume edilir.
	1. İşlemin başarı olması halinde *Customer Events channel* kuyruğuna bir event gönderilir. Başarısız durumda rollback uygulanır.
	1. Order Service'ta bu event consume edilerek nihai karara varılır; ya order tamamlanır ya da rollback uygulanır.
	
- Avantajları; 
	- Coupling azalır
	- Performance bottleneck azalır
	- Merkezi bir hata noktası olmadığından bakım maliyeti düşer.


- Dezavantajları; 
	- Hangi servisin, hangi kuyruğu dinlediğini takip etmek bir yerden sonra zorlaşmaktadır,bu nedenle yeni bir servis eklemek zor olabilir. 
	- Servisler, birbirlerinin kuyruklarını tükettikleri için aralarında döngüsel bir bağımlılık riski ortaya çıkabilir. 
	- Bir işlemi simüle etmek için tüm servislerin çalışıyor olması gerektiği için entegrasyon testi zordur.

### Command/Orchestration implementasyonu
- Bu yaklaşımda servisler arası distributed transaction, *Saga State Machine* ya da *Saga Orchestrator* ismi verilen **merkezi bir denetleyici** ile kontrol edilmektedir.
- Saga Orchestrator, servisler arasındaki tüm işlemleri yönetir ve olaylar doğrultusunda hangi işlemin gerçekleşeceğine karar verir.
- Saga Orchestrator, her kullanıcıdan gelen isteğe dair **uygulama state**'ini tutmakta ve gerektiğinde rollback işlemini bu sayede yapabilir.
- Örneğin; 
	1. Order Service'ta alınan POST isteği neticesinde sipariş oluşturulur. Sipariş durumu SUSPEND olarak kaydedilir.
	1. İlgili event, Saga Orchestrator'a gönderilir.
	1. Saga Orchestrator, EXECUTE_PAYMENT komutunu Payment Service'e, UPDATE_STOCK komutunu Stock Service'e ve ORDER_DELİVER komutunu da Delivery Service'e gönderir. (Misal olarak Stock Service'te stok miktarı yetersizse Saga Orchestrator'a OUT_OF_STOCK komutu gönderilir, ardından Saga Orchestrator bu komuttan hareketle başarısızlık olduğunu algılayıp rollback işlemlerini başlatır)
	1. Komutları işleyen servisler, bu işlemler neticesinde nihai durumu Saga Orchestrator'a gönderir.
	1. İşlemlerin başarı durumuna göre sipariş durumu değiştirilir.
	
- Avantajları; 
	- Karmaşık iş akışlarının yönetimini kolaylaştırır
	- Her servisin ve bu servislerin faaliyetlerinin üzerinde merkezi bir kontrol sağlar
	- Orchestrator, tek taraflı olarak servislere bağlı olduğundan dolayı döngüsel bağımlılıklar söz konusu değildir.
	- Her bir servisin diğer servislerle ilgili bilmesi gereken herhangi bir bilgiye ihtiyacı yoktur. Bu şekilde son derece bağımsız bir yapı sağlanır.

---

## Servisler Arası Mesaj Güvenliği

- Servisler arasındaki mesajlaşmayı daha güvenli bir hazneye almak için, bu süreci DB'ye işlemekteyiz.
- Bu sayede alıcıda ya da göndericide bir problem meydana gelirse, bu durumu daha kolay ele alabiliyoruz.

### Outbox Pattern

- Gönderici servisten alıcı servise gönderilen mesaj, **outbox table**'a kaydedilir.
- Ardından **Outbox Publisher Application** servisi, bu table'dan mesajları okuyarak, bunları event kuyrukları aracılığıyla ilgili servislere gönderir.
- Eğer servislerde anlık problemler söz konusu olursa da, yapılan işlem/ler bütünsel tutarlılık için geri alınmaktan ziyade, outbox table yardımıyla *giden kutusu*na kaydedilmeli ve düzenli aralıklarla bu tablo taranmalıdır.
- Burada Outbox Publisher Application önemli bir işleve sahiptir; tarama ve problemin çözülmesi durumunda gerekli servislere mesajı taşıma işlemlerini yürütür.
- Outbox Pattern sayesinde servisler arasındaki iletişi süreci; alıcı servisin yahut message broker'ın ayakta olup olmaması durumuna göre yaşanabilecek veri kaybı risklerinden arındırılarak daha güvenli hale getirilecek ve mesajın en az bir kez hedefe ulaşması garanti altına alınacaktır. Bu sayede de *Loosely Coupling* daha da artacaktır.
- Kritik mesajlara sahip olan her bir servis, kendi Outbox Table'ına sahip olmalıdır.

#### Hangi Durumlarda Kullanılır?

- Bir servis tarafından mesaj yayınlandığı esnada bu mesajı doğrudan hedef servise ya da message broker'a göndermek yerine bunları bir tabloya kaydederek ardından bir publisher aracılığıyla belirli zaman aralıklarında veya **CDC(Change Data Capture)** aracılığıyla hedef servislere iletmeyi amaçlayan bir pattern'dır.
- Mesajı en az bir kere hedefine ulaştırma garantisi sağlanır.
- Özellikle bir serviste aynı anda iki işlemin yapıldığı bir durumda Outbox Pattern çok önemli bir çözüm olarak karşımıza çıkar; Order oluştururken hem Orders tablosuna kayıt eklenip hem de OrderCreatedEvent isimli bir event message broker'a yazılıyorsa bu durum iki farklı yapı üzerinde kalıcı işlem yapıldığı için **Dual Write** olarak isimlendirilir.
- Dual Write durumlarında; yapılardan birinde işlem yapılıp, diğerinde yapılamaması gibi bir aksilik meydana gelirse bu yapıları dinleyen diğer servisler için ciddi bir veri tutarsızlığı oluşabilir. (uzun/kısa vadede)
- Outbox Pattern sayesinde bu gibi senaryolar karşısında son derece faydalı çözümler yakalayabiliriz. O yüzden, Dual Write durumlarında kesinlikle Outbox Pattern kullanılmalıdır.

#### Mesajlar Nasıl Publish Edilir?

##### Pulling Publisher

- Outbox Table'ın *belirli zaman aralıklarıyla* sorgulanıp publish işlem/lerinin yapılmasıdır. Bu uygulama basit bir console application yahut worker service olabilir.
- Bu yöntemin tek dezavantajı; sorgulama işleminin zaman aralığı ne kadar kısa ise, veri tabanı maliyeti de o kadar yükselecektir.
- Bir mesajın farklı publisher'lardan gönderilmesi gibi durumlarda, tekrar işleme durumlarına karşın MSSQL'deki UPLOCK ya da NoSQL'deki findAndModify gibi yöntemler uygulanabilir.

##### Transaction Log Trailing

- Outbox Table'ın bulunduğu veri tabanının transaction log'larının *belirli zaman aralıklarıyla* okunarak publish işlem/lerinin yapılmasıdır.

#### Örnek Senaryolar

- Sipariş verildikten sonra kullanıcıya e-mail göndermek
- Kullanıcı kaydı hakkında message broker'a bir event göndermek
- Bir sipariş verildikten sonra stoktaki ürün sayısını güncellemek

### Inbox Pattern

- İşlenecek mesajlar önce Inbox Table'a kaydedilip daha sonra publish edilirler.
- Örneğin; 
	- Gelen siparişi veri tabanına kaydedip ardından stok bilgisinin güncellenmesi için Outbox Table'a bu siparişle ilgili kaydın eklenip, publisher ile message broker'a gönderilmesi Outbox Pattern'dır.
	- Stok bilgisini güncellemekten sorumlu consumer'ın ilgili mesajı alıp, önce Inbox Table'a işleyip ardından işlemin gerçekleştirilmesi ise Inbox Pattern'dır.
- Outbox ve Inbox Pattern'lar, **Guaranteed Delivery Pattern**'ın birer bileşenidir!


### Idempotents Sorunsalı

- Outbox Pattern'ın görevini yaptıktan sonra, sıra mesajın durumunun güncellenmesine yahut silinmesine geldiğine tam bu esnada veri tabanı ile oluşabilecek bir iletişim hatası nedeniyle bu işlem gerçekleştirilemeyebilir.
- Böyle bir durumda, iletişim tekrar sağlandığında, işlenmiş olan mesaj tekrar işleme tabii tutulacak ve verisel tutarlılığı bozacaktır.
- Bu duruma karşın, consumer'ların **Idempotent** olarak tasarlanması bu olası problemi ortadan kaldıracaktır.
- Idempotent: bir mesaj birden çok kez yayınlansa dahi consumer'lar açısından aynı etkiye sahip bir işlevsellikte olmasını sağlayacak bir güvencenin sağlanması ve tutarlılığın korunması durumudur.

#### Idempotent İlkesi nasıl uygulanır?
- Unique Transaction Identifiers => Publish edilecek her bir mesaj için **ayırt edici bir değer**(anahtar, id, token) kullanılıp bu değer sayesinde bu mesajın daha önce tüketilip tüketilmediğini belirleyerek uygulanabilir. Bu değer, Outbox/Inbox table'a kaydedilmektedir. Genellikle bu yöntemi kullanıyoruz.
- Atomic Transaction => İşlemlere idempotent garantisi sağlayabilmek için tüm işlemi atomik hale getirebilir ve böylece *ya hep ya hiç prensibi*ni uygulayabiliriz. Maliyetli bir işlem olmakla birlikte, efektiftir.
- Caching of Transaction Results => İşlemin sonuçlarını cache'leyerek önceki işleme dair olan sonucu baz alarak bu işlemi gerçekleştiririz.
- Timestamp => Her işleme timestamp ekleyip belirli bir zaman aralığını kontrol ederek, bu işlemin tekrarlayıp tekrarlamadığını tespit edebiliriz.
	

#### Idempotent İlkesi'nin sağladığı avantajlar
- Tekrarlanabilirlik ve Güvenilirlik
- Hata Kurtarma ve Geri Alma
- Dağıtık Sistemlerde Güvenlik
- Performans İyileştirmesi
- Veritabanı İşlemlerinde Tutarlılık
- Sistemler Arası Entegrasyon