using MassTransit;
using Order.API.Context;
using Shared.OrderEvents;

namespace Order.API.Consumers;

public class OrderCompletedEventConsumer : IConsumer<OrderCompletedEvent>
{
	private readonly OrderDbContext _orderDbContext;

	public OrderCompletedEventConsumer(OrderDbContext orderDbContext)
	{
		_orderDbContext = orderDbContext;
	}

	public async Task Consume(ConsumeContext<OrderCompletedEvent> context)
	{
		var order = await _orderDbContext.Orders.FindAsync(context.Message.OrderId);

        if (order is null)
        {
			throw new NullReferenceException();
		}

		order.OrderStatus = Enums.OrderStatus.Completed;
		await _orderDbContext.SaveChangesAsync();
	}
}
