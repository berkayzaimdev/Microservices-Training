using MassTransit;
using MassTransit.Initializers;
using MongoDB.Driver;
using Shared.OrderEvents;
using Shared.Settings;
using Shared.StockEvents;
using Stock.API.Services;

namespace Stock.API.Consumers;

public class OrderCreatedEventConsumer : IConsumer<OrderCreatedEvent>
{
	private readonly MongoDbService _mongoDbService;
	private readonly ISendEndpointProvider _sendEndpointProvider;

	public OrderCreatedEventConsumer(MongoDbService mongoDbService, ISendEndpointProvider sendEndpointProvider)
	{
		_mongoDbService = mongoDbService;
		_sendEndpointProvider = sendEndpointProvider;
	}

	public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
	{
		List<bool> stockResult = [];

		var collection = _mongoDbService.GetCollection<Models.Stock>();

        foreach (var item in context.Message.OrderItems)
        {
			stockResult.Add(await(await collection.FindAsync(s => s.ProductId == item.ProductId && s.Count >= item.Count)).AnyAsync());
        }

		var sendEndpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri($"queue:{RabbitMQSettings.StateMachineQueue}"));

        if (stockResult.TrueForAll(s => s.Equals(true)))
        {
            foreach (var item in context.Message.OrderItems)
            {
				var stock = await (await collection.FindAsync(s => s.ProductId == item.ProductId)).FirstOrDefaultAsync();
				stock.Count -= item.Count;

				await collection.FindOneAndReplaceAsync(s => s.ProductId == item.ProductId, stock);
			}

			StockReservedEvent stockReservedEvent = new(context.Message.CorrelationId)
			{
				OrderItems = context.Message.OrderItems
			};

			await sendEndpoint.Send(stockReservedEvent);
        }

        else
        {
			StockNotReservedEvent stockNotReservedEvent = new(context.Message.CorrelationId)
			{
				Message = "Stok yetersiz!"
			};

			await sendEndpoint.Send(stockNotReservedEvent);
		}
    }
}
