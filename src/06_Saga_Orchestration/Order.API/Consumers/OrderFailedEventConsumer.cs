using MassTransit;
using Order.API.Context;
using Shared.OrderEvents;

namespace Order.API.Consumers;

public class OrderFailedEventConsumer : IConsumer<OrderFailedEvent>
{
	private readonly OrderDbContext _orderDbContext;

	public OrderFailedEventConsumer(OrderDbContext orderDbContext)
	{
		_orderDbContext = orderDbContext;
	}

	public async Task Consume(ConsumeContext<OrderFailedEvent> context)
	{
		var order = await _orderDbContext.Orders.FindAsync(context.Message.OrderId);

		if (order is null)
		{
			throw new NullReferenceException();
		}

		order.OrderStatus = Enums.OrderStatus.Failed;
		await _orderDbContext.SaveChangesAsync();
	}
}