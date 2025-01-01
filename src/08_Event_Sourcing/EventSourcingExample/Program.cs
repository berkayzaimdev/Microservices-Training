using System.Text.Json;
using EventStore.Client;

#region İnceleme

// string connectionString = "esbd://admin:changeit@localhost:2113?tls=false&tlsVerifyCert=false";
//
// var settings = EventStoreClientSettings.Create(connectionString);
// var client = new EventStoreClient(settings);

// var orderPlacementEvent = new OrderPlacedEvent
// {
//     OrderId = 1,
//     TotalAmount = 200
// };

// EventData eventData = new EventData(
//     eventId: Uuid.NewUuid(),
//     type: orderPlacementEvent.GetType().Name,
//     data: JsonSerializer.SerializeToUtf8Bytes(orderPlacementEvent)
// );

// await client.AppendToStreamAsync(
//     streamName: "order-stream",
//     expectedState: StreamState.Any, // akış durumu herhangi bir akış
//     eventData: new[] { eventData }
// );

// var events = client.ReadStreamAsync(
//     streamName: "order-stream",
//     direction: Direction.Forwards, // artan index yönünde okuma sağlar
//     revision: StreamPosition.Start // akışın başlangıcından başlayalım
// );


// await client.SubscribeToStreamAsync(
//     streamName: "order-stream",
//     start: FromStream.Start,
//     eventAppeared: async (streamSubscription, resolvedEvent, cancellationToken) =>
//     {
//         OrderPlacedEvent @event = JsonSerializer.Deserialize<OrderPlacedEvent>(resolvedEvent.Event.Data.ToArray())!; //bytearray to json
//         await Console.Out.WriteLineAsync(JsonSerializer.Serialize(@event));
//     },
//     subscriptionDropped: (streamSubscription, resolvedEvent, cancellationToken) => Console.WriteLine("disconnected")
// ); 
// // tüm event'ları okuyup console a yazdırır
//
// class OrderPlacedEvent
// {
//     public int OrderId { get; set; }
//     public int TotalAmount { get; set; }
// }

#endregion

#region Örnek Uygulama

// string connectionString = "esbd://admin:changeit@localhost:2113?tls=false&tlsVerifyCert=false";
//
// var settings = EventStoreClientSettings.Create(connectionString);
// var client = new EventStoreClient(settings);

// var orderPlacementEvent = new OrderPlacedEvent
// {
//     OrderId = 1,
//     TotalAmount = 200
// };

// EventData eventData = new EventData(
//     eventId: Uuid.NewUuid(),
//     type: orderPlacementEvent.GetType().Name,
//     data: JsonSerializer.SerializeToUtf8Bytes(orderPlacementEvent)
// );

// await client.AppendToStreamAsync(
//     streamName: "order-stream",
//     expectedState: StreamState.Any, // akış durumu herhangi bir akış
//     eventData: new[] { eventData }
// );

// var events = client.ReadStreamAsync(
//     streamName: "order-stream",
//     direction: Direction.Forwards, // artan index yönünde okuma sağlar
//     revision: StreamPosition.Start // akışın başlangıcından başlayalım
// );


// await client.SubscribeToStreamAsync(
//     streamName: "order-stream",
//     start: FromStream.Start,
//     eventAppeared: async (streamSubscription, resolvedEvent, cancellationToken) =>
//     {
//         OrderPlacedEvent @event = JsonSerializer.Deserialize<OrderPlacedEvent>(resolvedEvent.Event.Data.ToArray())!; //bytearray to json
//         await Console.Out.WriteLineAsync(JsonSerializer.Serialize(@event));
//     },
//     subscriptionDropped: (streamSubscription, resolvedEvent, cancellationToken) => Console.WriteLine("disconnected")
// ); 
// // tüm event'ları okuyup console a yazdırır

AccountCreatedEvent accountCreatedEvent1 = new AccountCreatedEvent()
{
    AccountId = "12345",
    CustomerId = "45678",
    StartBalance = 0,
    OccureDate = DateTime.Now,
};

AccountCreatedEvent accountCreatedEvent2 = new AccountCreatedEvent()
{
    AccountId = "22222",
    CustomerId = "90123",
    StartBalance = 300,
    OccureDate = DateTime.Now,
};

MoneyDepositedEvent moneyDepositedEvent1 = new()
{
    AccountId = "12345",
    Amount = 7000,
    OccureDate = DateTime.Now,
};

MoneyDepositedEvent moneyDepositedEvent2 = new()
{
    AccountId = "12345",
    Amount = 2220,
    OccureDate = DateTime.Now,
};

MoneyDepositedEvent moneyDepositedEvent3 = new()
{
    AccountId = "22222",
    Amount = 15000,
    OccureDate = DateTime.Now,
};

MoneyWithdrawnEvent moneyWithdrawnEvent1 = new()
{
    AccountId = "12345",
    Amount = 500,
    OccureDate = DateTime.Now,
};

MoneyTransferredEvent moneyTransferredEvent1 = new()
{
    AccountId = "12345",
    TargetAccountId = "22222",
    Amount = 5000,
    OccureDate = DateTime.Now,
};

MoneyWithdrawnEvent moneyWithdrawnEvent2 = new()
{
    AccountId = "22222",
    Amount = 350,
    OccureDate = DateTime.Now,
};

MoneyTransferredEvent moneyTransferredEvent2 = new()
{
    AccountId = "22222",
    TargetAccountId = "12345",
    Amount = 1234,
    OccureDate = DateTime.Now,
};

EventStoreService eventStoreService = new();

await eventStoreService.AppendToStreamAsync(
    streamName: $"costumer-{accountCreatedEvent1.CustomerId}-stream",
    eventDatas: new[]
    {
        eventStoreService.GenerateEventData(accountCreatedEvent1),
        eventStoreService.GenerateEventData(moneyDepositedEvent1),
        eventStoreService.GenerateEventData(moneyDepositedEvent2),
        eventStoreService.GenerateEventData(moneyWithdrawnEvent1),
        eventStoreService.GenerateEventData(moneyTransferredEvent1)
    }
);

await eventStoreService.AppendToStreamAsync(
    streamName: $"costumer-{accountCreatedEvent2.CustomerId}-stream",
    eventDatas: new[]
    {
        eventStoreService.GenerateEventData(accountCreatedEvent2),
        eventStoreService.GenerateEventData(moneyDepositedEvent3),
        eventStoreService.GenerateEventData(moneyWithdrawnEvent2),
        eventStoreService.GenerateEventData(moneyTransferredEvent2)
    }
);

BalanceInfo balanceInfo = new();

Func<StreamSubscription, ResolvedEvent, CancellationToken, Task> eventAppearedFunc =     
    async (ss, re, ct) =>
{
    string eventType = re.Event.EventType;
    object @event = JsonSerializer.Deserialize(re.Event.Data.ToArray(), Type.GetType(eventType)!)!;

    switch (@event)
    {
        case AccountCreatedEvent e:
            balanceInfo.AccountId = e.AccountId;
            balanceInfo.Balance = e.StartBalance;
            break;
        case MoneyDepositedEvent e:
            balanceInfo.Balance += e.Amount;
            break;
        case MoneyWithdrawnEvent e:
            balanceInfo.Balance -= e.Amount;
            break;
        case MoneyTransferredEvent e:
            balanceInfo.Balance -= e.Amount;
            break;
    }

    await Console.Out.WriteLineAsync("***************BALANCE***************");
    await Console.Out.WriteLineAsync(JsonSerializer.Serialize(balanceInfo));
    await Console.Out.WriteLineAsync("***************BALANCE***************");
    await Console.Out.WriteLineAsync("");
    await Console.Out.WriteLineAsync("");
};

await eventStoreService.SubscribeToStreamAsync($"costumer-{accountCreatedEvent1.CustomerId}-stream", eventAppearedFunc);
await eventStoreService.SubscribeToStreamAsync($"costumer-{accountCreatedEvent2.CustomerId}-stream", eventAppearedFunc);

class EventStoreService
{
    EventStoreClientSettings GetEventStoreClientSettings(string connectionString = "esbd://admin:changeit@localhost:2113?tls=false&tlsVerifyCert=false") 
        => EventStoreClientSettings.Create(connectionString);
    
    EventStoreClient Client => new EventStoreClient(GetEventStoreClientSettings());

    public async Task AppendToStreamAsync(string streamName, IEnumerable<EventData> eventDatas) =>
        await Client.AppendToStreamAsync(
            streamName: streamName,
            eventData: eventDatas,
            expectedState: StreamState.Any
        );

    public EventData GenerateEventData(object @event) => new(
        eventId: Uuid.NewUuid(),
        type: @event.GetType().Name,
        data: JsonSerializer.SerializeToUtf8Bytes(@event)
    );
    
    public async Task SubscribeToStreamAsync(string streamName, Func<StreamSubscription, ResolvedEvent, CancellationToken, Task> eventAppeared) 
        => await Client.SubscribeToStreamAsync(
            streamName: streamName, 
            start: FromStream.Start, 
            eventAppeared: eventAppeared, // akışa bağlanan event
            subscriptionDropped: (x,y,z) => Console.WriteLine("disconnected") // akıştan kopunca çağrılır
        );
}

class BalanceInfo
{
    public string AccountId { get; set; }
    public int Balance { get; set; } 
}

class AccountCreatedEvent
{
    public string AccountId { get; set; }
    public string CustomerId { get; set; }
    public int StartBalance { get; set; }
    public DateTime OccureDate { get; set; }
}

class MoneyDepositedEvent
{
    public string AccountId { get; set; }
    public int Amount { get; set; }
    public DateTime OccureDate { get; set; }
}

class MoneyWithdrawnEvent
{
    public string AccountId { get; set; }
    public int Amount { get; set; }
    public DateTime OccureDate { get; set; }
}

class MoneyTransferredEvent
{
    public string AccountId { get; set; }
    public string TargetAccountId { get; set; }
    public int Amount { get; set; }
    public DateTime OccureDate { get; set; }
}

#endregion