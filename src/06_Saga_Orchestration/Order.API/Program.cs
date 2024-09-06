using MassTransit;
using Microsoft.EntityFrameworkCore;
using Order.API.Consumers;
using Order.API.Context;
using Order.API.ViewModels;
using Shared.Messages;
using Shared.OrderEvents;
using Shared.Settings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMassTransit(configurator =>
{
	configurator.AddConsumer<OrderCompletedEventConsumer>();
	configurator.AddConsumer<OrderFailedEventConsumer>();

	configurator.UsingRabbitMq((context, _configure) =>
	{
		_configure.Host("...");

		_configure.ReceiveEndpoint(RabbitMQSettings.Order_OrderCompletedEventQueue, e => e.ConfigureConsumer<OrderCompletedEventConsumer>(context));
		_configure.ReceiveEndpoint(RabbitMQSettings.Order_OrderFailedEventQueue, e => e.ConfigureConsumer<OrderFailedEventConsumer>(context));
	});
});

builder.Services.AddDbContext<OrderDbContext>(opts => opts.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Database=SagaOrchestration;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False"));

var app = builder.Build();

app.MapPost("/create-order", async (CreateOrderVM model, OrderDbContext context, ISendEndpointProvider sendEndpointProvider) =>
{
	Order.API.Models.Order order = new()
	{
		BuyerId = model.BuyerId,
		CreatedDate = DateTime.UtcNow,
		OrderStatus = Order.API.Enums.OrderStatus.Suspend,
		TotalPrice = model.OrderItems.Sum(oi => oi.Price * oi.Count),
		OrderItems = model.OrderItems.Select(oi => new Order.API.Models.OrderItem()
		{
			ProductId = oi.ProductId,
			Price = oi.Price,
			Count = oi.Count
		}).ToList()
	};

	await context.Orders.AddAsync(order);
	await context.SaveChangesAsync();

	OrderStartedEvent orderStartedEvent = new()
	{
		BuyerId = model.BuyerId,
		OrderId = order.Id,
		TotalPrice = model.OrderItems.Sum(oi => oi.Price * oi.Count),
		OrderItems = model.OrderItems.Select(oi => new OrderItemMessage()
		{
			ProductId = oi.ProductId,
			Price = oi.Price,
			Count = oi.Count
		}).ToList()
	};

	var sendEndpoint = await sendEndpointProvider.GetSendEndpoint(new Uri($"queue:{RabbitMQSettings.StateMachineQueue}"));

	await sendEndpoint.Send(orderStartedEvent); // Order'ın başladığına dair event'ı, state machine kuyruğuna gönderiyoruz
});

app.Run();
