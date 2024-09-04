using MassTransit;
using Shared.Events;

namespace Payment.API.Consumers;

public class StockReservedEventConsumer : IConsumer<StockReservedEvent>
{
    private readonly IPublishEndpoint publishEndpoint;
	public async Task Consume(ConsumeContext<StockReservedEvent> context)
	{
        if (true)
        {
            PaymentCompletedEvent paymentCompletedEvent = new()
            {
                OrderId = context.Message.OrderId
            };

            await publishEndpoint.Publish(paymentCompletedEvent);
        }
        else
        {
			PaymentFailedEvent paymentFailedEvent = new()
			{
				OrderId = context.Message.OrderId,
                Message = "Yetersiz bakiye",
                OrderItems = context.Message.OrderItems
			};

			await publishEndpoint.Publish(paymentFailedEvent);
		}
    }
}
