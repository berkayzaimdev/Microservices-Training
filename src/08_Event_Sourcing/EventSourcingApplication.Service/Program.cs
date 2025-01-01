using EventSourcingApplication.Shared.Services;
using EventSourcingApplication.Shared.Services.Abstractions;
using EventSourcingApplication.Service.Workers;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHostedService<EventStoreBackgroundService>();
builder.Services.AddSingleton<IEventStoreService, EventStoreService>();
builder.Services.AddSingleton<IMongoDbService, MongoDbService>();

var host = builder.Build();
host.Run();