using MassTransit;
using Microsoft.EntityFrameworkCore;
using Order.API.Models.Contexts;
using Order.API.Models.Entities;
using Order.API.ViewModels;
using Shared;
using Shared.Events;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext<OrderDbContext>(options => options.UseSqlServer("..."));

builder.Services.AddMassTransit(configurator =>
{
	configurator.UsingRabbitMq((context, _configure) =>
	{
		_configure.Host("...");
	});
});

var app = builder.Build();


app.MapPost("/create-order", async (CreateOrderVM model, OrderDbContext orderDbContext, ISendEndpointProvider sendEndpointProvider) =>
{
	Order.API.Models.Entities.Order order = new()
	{
		BuyerId = model.BuyerId,
		CreatedDate = DateTime.UtcNow,
		TotalPrice = model.OrderItems.Sum(oi => oi.Count * oi.Price),
		OrderItems = model.OrderItems.Select(oi => new Order.API.Models.Entities.OrderItem
		{
			Price = oi.Price,
			Count = oi.Count,
			ProductId = oi.ProductId,
		}).ToList(),
	};

	await orderDbContext.Orders.AddAsync(order);
	await orderDbContext.SaveChangesAsync();

	var idempotentToken = Guid.NewGuid();
	OrderCreatedEvent orderCreatedEvent = new()
	{
		BuyerId = order.BuyerId,
		OrderId = order.Id,
		TotalPrice = model.OrderItems.Sum(oi => oi.Count * oi.Price),
		OrderItems = model.OrderItems.Select(oi => new Shared.Datas.OrderItem()
		{
			Price = oi.Price,
			Count = oi.Count,
			ProductId= oi.ProductId
		})
		.ToList(),
		IdempotentToken = idempotentToken
	};

	#region w/o Outbox Pattern

	//var sendEndpoint = await sendEndpointProvider.GetSendEndpoint(new Uri($"queue:{RabbitMQSettings.Stock_OrderCreatedEvent}"));
	//await sendEndpoint.Send<OrderCreatedEvent>(orderCreatedEvent);

	#endregion

	#region w/ Outbox Pattern

	OrderOutbox orderOutbox = new()
	{
		OccuredOn = DateTime.UtcNow,
		ProcessedDate = null,
		Type = orderCreatedEvent.GetType().Name,
		Payload = JsonSerializer.Serialize(orderCreatedEvent),
		IdempotentToken = idempotentToken
	};

	await orderDbContext.OrderOutboxes.AddAsync(orderOutbox);
	await orderDbContext.SaveChangesAsync();

	#endregion
});

app.Run();