using MassTransit;
using Microsoft.EntityFrameworkCore;
using Order.API.Models.Context;
using Shared.Events;

namespace Order.API.Consumers;

public class PaymentFailedEventConsumer : IConsumer<PaymentFailedEvent>
{
	private readonly OrderAPIDbContext _context;

	public PaymentFailedEventConsumer(OrderAPIDbContext context)
	{
		_context = context;
	}

	public async Task Consume(ConsumeContext<PaymentFailedEvent> context)
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
