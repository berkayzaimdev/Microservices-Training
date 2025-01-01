using System.Reflection;
using System.Text.Json;
using EventSourcingApplication.Shared.Events;
using EventSourcingApplication.Shared.Models;
using EventSourcingApplication.Shared.Services.Abstractions;
using MongoDB.Bson;
using MongoDB.Driver;

namespace EventSourcingApplication.Service.Workers;

public class EventStoreBackgroundService(IEventStoreService eventStoreService, IMongoDbService mongoDbService) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await eventStoreService.SubscribeToStreamAsync("products-stream",
        async (streamSubscription, resolvedEvent, cancellationToken) =>
        {
            string eventType = resolvedEvent.Event.EventType;
            object @event = JsonSerializer.Deserialize(resolvedEvent.Event.Data.ToArray(), Assembly.Load("Shared").GetTypes().FirstOrDefault(t => t.Name.Equals(eventType, StringComparison.OrdinalIgnoreCase)));


            var productCollection = mongoDbService.GetCollection<Product>("Products");
            
            switch (@event)
            {
                case NewProductAddedEvent e:
                    var hasProduct = await( await productCollection.FindAsync(p => p.Id.ToString() == e.ProductId)).AnyAsync();

                    if (!hasProduct)
                    {
                        await productCollection.InsertOneAsync(new()
                        {
                            Id = ObjectId.GenerateNewId(),
                            ProductName = e.ProductName,
                            IsAvailable = e.IsAvailable,
                            Price = e.InitialPrice,
                            Count = e.InitialCount
                        });
                    }
                    
                    break;
            }
        } );


        
        throw new NotImplementedException();
    }
}