using MassTransit;
using Shared.Events;
using Stock.API.Services;
using MongoDB.Driver;

namespace Stock.API.Consumers;

public class PaymentFailedEventConsumer : IConsumer<PaymentFailedEvent>
{
	private readonly MongoDBService _mongoService;

	public PaymentFailedEventConsumer(MongoDBService mongoService)
	{
		_mongoService = mongoService;
	}

	public async Task Consume(ConsumeContext<PaymentFailedEvent> context)
	{
		var stocks = _mongoService.GetCollection<Models.Stock>()!;

        foreach (var orderItem in context.Message.OrderItems)
        {
			var stock = await (await stocks.FindAsync(s => s.ProductId == orderItem.ProductId)).FirstOrDefaultAsync();

			stock.Quantity += orderItem.Quantity;

			await stocks.FindOneAndReplaceAsync(s => s.ProductId == orderItem.ProductId, stock);
        }
    }
}


