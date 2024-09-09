using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared.Events;
using Stock.Service.Models.Contexts;
using Stock.Service.Models.Entities;
using System.Text.Json;

namespace Stock.Service.Consumers;
public class OrderCreatedEventConsumer : IConsumer<OrderCreatedEvent>
{
	private readonly StockDbContext _context;

	public OrderCreatedEventConsumer(StockDbContext context)
	{
		_context = context;
	}

	public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
	{
		await _context.OrderInboxes.AddAsync(new Models.Entities.OrderInbox()
		{
			Processed = false,
			Payload = JsonSerializer.Serialize(context.Message)
		});

		await _context.SaveChangesAsync();

		List<OrderInbox> orderInboxes = await _context.OrderInboxes.Where(i => !i.Processed).ToListAsync();

        foreach (var orderInbox in orderInboxes)
        {
			var orderCreatedEvent = JsonSerializer.Deserialize<OrderCreatedEvent>(orderInbox.Payload);

			await Console.Out.WriteLineAsync($"{orderCreatedEvent.OrderId} orderID değerine karşılık olan siparişin stok işlemleri başarıyla tamamlanmıştır.");
			orderInbox.Processed = true;			
		}

		await _context.SaveChangesAsync();
	}
}