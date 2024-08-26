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
