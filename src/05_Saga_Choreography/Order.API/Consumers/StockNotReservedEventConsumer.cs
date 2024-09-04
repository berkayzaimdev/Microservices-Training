using MassTransit;
using Order.API.Models.Context;
using Shared.Events;

namespace Order.API.Consumers;

public class StockNotReservedEventConsumer : IConsumer<StockNotReservedEvent>
{
	private readonly OrderAPIDbContext _context;

	public StockNotReservedEventConsumer(OrderAPIDbContext context)
	{
		_context = context;
	}

	public async Task Consume(ConsumeContext<StockNotReservedEvent> context)
	{
		var order = await _context.Orders.FindAsync(context.Message.OrderId);

		if (order is null)
		{
			throw new NotImplementedException();
		}

		order.OrderStatus = Enums.OrderStatus.Fail;
		await _context.SaveChangesAsync();
	}
}
