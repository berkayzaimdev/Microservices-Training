using MassTransit;
using Shared;
using Stock.API.Consumers;
using Stock.API.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMassTransit(configurator =>
{
	configurator.UsingRabbitMq((context, _configurator) =>
	{
		_configurator.Host(builder.Configuration["RabbitMQ"]);
		_configurator.ReceiveEndpoint(RabbitMQSettings.Stock_OrderCreatedEventQueue, e => e.ConfigureConsumer<OrderCreatedEventConsumer>(context));
	});
});

builder.Services.AddSingleton<MongoDBService>();

var app = builder.Build();

app.Run();
