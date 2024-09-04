namespace Coordinator.Services.Abstractions;

public interface ITransactionService
{
	Task<Guid> CreateTransaction(); // transaction oluştu mu oluşmadı mı, buna dair guid dönecek
	Task PrepareServices(Guid transactionId); // servislerin hazır olup olmadığını, prepare aşamasını geçip geçmediklerini sağlayacağımız metot 
	Task<bool> CheckReadyServices(Guid transactionId); // ikinci aşamaya geçip geçmeyeceğimizi kontrol edeceğimiz metot. nodestate durumuna göre bool değer dönecek
	Task Commit(Guid transactionId); // commit mesajı göndereceğimiz metot
	Task<bool> CheckTransactionStateServices(Guid transactionId); // servislerin transaction durumunu kontrol edip, ilgili operasyonun başarıyla sağlanıp sağlanmadığına göre bool değer döneceğimiz metot
	Task Rollback(Guid transactionId); // başarısız olunursa geri dönüş için metot
}
