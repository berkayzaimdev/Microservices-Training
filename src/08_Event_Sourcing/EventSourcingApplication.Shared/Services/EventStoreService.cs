using System.Text.Json;
using EventSourcingApplication.Shared.Services.Abstractions;
using EventStore.Client;

namespace EventSourcingApplication.Shared.Services;

public class EventStoreService : IEventStoreService
{
    private readonly EventStoreClient _client;
    public EventStoreService()
    {
        var settings = EventStoreClientSettings.Create("esdb://admin:changeit@localhost:2113?tls=false&tlsVerifyCert=false");
        _client = new(settings);       
    }

    public EventData GenerateEventData(object @event)
        => new(
            eventId: Uuid.NewUuid(),
            type: @event.GetType().Name,
            data: JsonSerializer.SerializeToUtf8Bytes(@event)
        );

    public async Task AppendToStreamAsync(string streamName, IEnumerable<EventData> eventDatas)
        => await _client.AppendToStreamAsync(
            streamName: streamName,
            eventData: eventDatas,
            expectedState: StreamState.Any
        );

    public async Task SubscribeToStreamAsync(string streamName,
        Func<StreamSubscription, ResolvedEvent, CancellationToken, Task> eventAppeared)
        => await _client.SubscribeToStreamAsync(
            streamName: streamName,
            start: FromStream.Start,
            eventAppeared: eventAppeared,
            subscriptionDropped: (streamSubscription, streamSubscriptionDroppedReason, exception)
                => Console.WriteLine($"Disconnected")
        );
}