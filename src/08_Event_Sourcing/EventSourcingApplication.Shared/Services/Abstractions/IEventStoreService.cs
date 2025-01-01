using EventStore.Client;

namespace EventSourcingApplication.Shared.Services.Abstractions;

public interface IEventStoreService
{
    EventData GenerateEventData(object @event);
    Task AppendToStreamAsync(string streamName, IEnumerable<EventData> eventDatas);
    Task SubscribeToStreamAsync(string streamName, Func<StreamSubscription, ResolvedEvent, CancellationToken, Task> eventAppeared);
}