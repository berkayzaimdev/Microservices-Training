using MassTransit;
using MongoDB.Driver;
using Shared;
using Shared.Events;
using Stock.API.Models;
using Stock.API.Services;

namespace Stock.API.Consumers;

public class OrderCreatedEventConsumer : IConsumer<OrderCreatedEvent>
{
	readonly MongoDBService _mongoDBService;
    private readonly ISendEndpointProvider _sendEndpointProvider; // hedef odaklı mesaj yollama için işlevsel
    private readonly IPublishEndpoint _publishEndpoint;

	public OrderCreatedEventConsumer(MongoDBService mongoDBService, ISendEndpointProvider sendEndpointProvider, IPublishEndpoint publishEndpoint)
	{
		_mongoDBService = mongoDBService;
		_sendEndpointProvider = sendEndpointProvider;
		_publishEndpoint = publishEndpoint;
	}

	public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
	{
		List<bool> stockResults = new();
		var collection = _mongoDBService.GetCollection<Models.Stock>()!;

        foreach (var item in context.Message.OrderItems)
        {
			stockResults.Add(await (await collection.FindAsync(s => s.ProductId == item.ProductId && s.Quantity > item.Quantity)).AnyAsync());
        }

        if (stockResults.TrueForAll(s => s.Equals(true)))
        {
            foreach (var orderItem in context.Message.OrderItems)
            {
                var stock = await (await collection.FindAsync(s => s.ProductId == orderItem.ProductId)).FirstOrDefaultAsync();
                stock.Quantity -= orderItem.Quantity;

                await collection.FindOneAndReplaceAsync(s => s.ProductId == orderItem.ProductId, stock);

			}

            var sendEndpoint = await _sendEndpointProvider.GetSendEndpoint(new($"queue:{RabbitMQSettings.Payment_StockReservedEventQueue}"));
            var stockReservedEvent = new StockReservedEvent
            {
                BuyerId = context.Message.BuyerId,
                OrderId = context.Message.OrderId,
                TotalPrice = context.Message.TotalPrice,
                OrderItems = context.Message.OrderItems,
            };

            await sendEndpoint.Send(stockReservedEvent); // hedef olan kuyruğa mesaj gönder (1 kuyruk)
        }
        else
        {
            var stockNotReservedEvent = new StockNotReservedEvent
            {
				BuyerId = context.Message.BuyerId,
				OrderId = context.Message.OrderId,
                Message = "Stok miktarı yetersiz.."
			};

            await _publishEndpoint.Publish(stockNotReservedEvent); // bu event'a subscribe olan tüm kuyruklara gönder (n kuyruk)
		}
    }
}
