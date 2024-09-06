using MassTransit;
using SagaStateMachine.Service.StateInstances;
using Shared.Messages;
using Shared.OrderEvents;
using Shared.PaymentEvents;
using Shared.Settings;
using Shared.StockEvents;

namespace SagaStateMachine.Service.StateMachine;

public class OrderStateMachine : MassTransitStateMachine<OrderStateInstance> // machine, hangi instance'a göre davranış sergileyecek bunu generic olarak verdik
{
    public Event<OrderStartedEvent> OrderStartedEvent { get; set; }
    public Event<StockReservedEvent> StockReservedEvent { get; set; }
    public Event<StockNotReservedEvent> StockNotReservedEvent { get; set; }
    public Event<PaymentCompletedEvent> PaymentCompletedEvent { get; set; }
    public Event<PaymentFailedEvent> PaymentFailedEvent { get; set; }

    public State OrderCreated { get; set; }
    public State StockReserved { get; set; }
    public State StockNotReserved { get; set; }
    public State PaymentCompleted { get; set; }
    public State PaymentFailed { get; set; }



    public OrderStateMachine()
    {
        InstanceState(instance => instance.CurrentState); // durumu, CurrentState property'sinde tutuyoruz

		#region Event'larla ilgili aksiyonlarımızı tamamladık

		// yeni bir sipariş oluşturduğumuz için correlationId'ye ihtiyaç duyduk. işlemlerimiz bu id üzerinden yürüyecek
		// gelen event tetikleyici bir event olan OrderStartedEvent ise, 
		Event(() => OrderStartedEvent,
            orderStateInstance => orderStateInstance.CorrelateBy<int>(db => db.OrderId, @event => @event.Message.OrderId) // orderId = int olduğu için generic olarak int verdik. correlationId'ye ait olan Guid ile karıştırmayalım
            // veritabanında böyle bir işleşme var mı? varsa zaten bir state tutulduğu için yeni bir correlationId oluşturmaya gerek yok, yoksa yeni id oluştur
            .SelectId(e => Guid.NewGuid())
        );

        // tetikleyici event değil
        // hangi duruma correlationId ile bağlı ise, onu ifade edecektir
        Event(() => StockReservedEvent,
            orderStateInstance => orderStateInstance.CorrelateById(@event => @event.Message.CorrelationId)
        );

		Event(() => StockNotReservedEvent,
	        orderStateInstance => orderStateInstance.CorrelateById(@event => @event.Message.CorrelationId)
        );

		Event(() => PaymentCompletedEvent,
	        orderStateInstance => orderStateInstance.CorrelateById(@event => @event.Message.CorrelationId)
        );

		Event(() => PaymentFailedEvent,
	        orderStateInstance => orderStateInstance.CorrelateById(@event => @event.Message.CorrelationId)
        );

        #endregion

        #region OrderStarted state

        // ne zaman ki ilk tetikleyici event'ımız geldiğinde, gerekli işlemleri yap
        Initially(When(OrderStartedEvent)
            .Then(context =>
            {
                context.Instance.OrderId = context.Data.OrderId; // event'taki veride bulunan OrderID'yi, state instance'taki OrderID'ye yazdık
                context.Instance.BuyerId = context.Data.BuyerId;
                context.Instance.TotalPrice = context.Data.TotalPrice;
                context.Instance.CreatedDate = DateTime.UtcNow;
            })
            .TransitionTo(OrderCreated) // state'i, Created olarak ayarla
            .Send(new Uri($"queue:{RabbitMQSettings.Stock_OrderCreatedEventQueue}"), // event'ın akabinde gönderilecek mesaj ve hedef kuyruk
			context => new OrderCreatedEvent(context.Instance.CorrelationId)
            {
                OrderItems = context.Data.OrderItems
            }));

		// context Instance = state instance
		// context Data = o anki event'tan gelen data

		#endregion

		#region OrderCreated state

		During(OrderCreated // o anki durum OrderCreated ise;
			,When(StockReservedEvent) // VE gelen event StockReservedEvent ise;
	        .TransitionTo(StockReserved) // durumu StockReserved olarak ayarla
			.Send(new Uri($"queue:{RabbitMQSettings.Payment_StartedEventQueue}"), // ve kuyruğa mesaj gönder, topu Payment.API'ye at
	        context => new PaymentStartedEvent(context.Instance.CorrelationId)
	        {
		        OrderItems = context.Data.OrderItems,
                TotalPrice = context.Instance.TotalPrice
	        })

            ,When(StockNotReservedEvent) // Yok şayet gelen event StockNotReservedEvent ise;
			.TransitionTo(StockNotReserved) // durumu StockNotReserved olarak ayarla
			.Send(new Uri($"queue:{RabbitMQSettings.Order_OrderFailedEventQueue}"),
			context => new OrderFailedEvent()
			{
				OrderId = context.Instance.OrderId,
				Message = context.Data.Message
			}));

		#endregion

		#region StockReserved state

		During(StockReserved 
			,When(PaymentCompletedEvent)
			.TransitionTo(PaymentCompleted)
			.Send(new Uri($"queue:{RabbitMQSettings.Order_OrderCompletedEventQueue}"),
			context => new OrderCompletedEvent()
			{
				OrderId = context.Instance.OrderId
			})
			.Finalize() 
			,When(PaymentFailedEvent) 
			.TransitionTo(PaymentFailed) 
			.Send(new Uri($"queue:{RabbitMQSettings.Order_OrderFailedEventQueue}"),
			context => new StockRollbackMessage()
			{
				OrderItems = context.Data.OrderItems
			})); // burada finalize çağırmıyoruz çünkü olayın metriğine ihtiyacımız var. bu sebeple veriler DB'de kalıyor

		SetCompletedWhenFinalized(); // state instance'ı, db'den siliyoruz. çünkü artık işlem tamamlanmıştır

		#endregion
	}
}
