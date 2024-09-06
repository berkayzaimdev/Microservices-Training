using MassTransit;
using MongoDB.Driver;
using Shared.Messages;
using Stock.API.Services;

namespace Stock.API.Consumers;

public class StockRollbackMessageConsumer : IConsumer<StockRollbackMessage>
{
	private readonly MongoDbService _mongoDbService;

	public StockRollbackMessageConsumer(MongoDbService mongoDbService)
	{
		_mongoDbService = mongoDbService;
	}

	public async Task Consume(ConsumeContext<StockRollbackMessage> context)
	{
		var collection = _mongoDbService.GetCollection<Models.Stock>();

		foreach (var item in context.Message.OrderItems)
		{
			var stock = await (await collection.FindAsync(s => s.ProductId == item.ProductId && s.Count >= item.Count)).FirstOrDefaultAsync();

			stock.Count += item.Count;

			await collection.FindOneAndReplaceAsync(s => s.ProductId == item.ProductId, stock);
		}
	}
}
