using MassTransit;
using Microsoft.EntityFrameworkCore;
using Order.API.Consumers;
using Order.API.Models;
using Order.API.Models.Context;
using Order.API.ViewModels;
using Shared;
using Shared.Events;
using Shared.Messages;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMassTransit(configurator =>
{
	configurator.AddConsumer<PaymentCompletedEventConsumer>();
	configurator.AddConsumer<PaymentFailedEventConsumer>();
	configurator.AddConsumer<StockNotReservedEventConsumer>();
	configurator.UsingRabbitMq((context, _configurator) =>
	{
		_configurator.Host(builder.Configuration["RabbitMQ"]);
		_configurator.ReceiveEndpoint(RabbitMQSettings.Order_PaymentCompletedEventQueue, e => e.ConfigureConsumer<PaymentCompletedEventConsumer>(context));		
		_configurator.ReceiveEndpoint(RabbitMQSettings.Order_PaymentFailedEventQueue, e => e.ConfigureConsumer<PaymentFailedEventConsumer>(context));
		_configurator.ReceiveEndpoint(RabbitMQSettings.Order_PaymentFailedEventQueue, e => e.ConfigureConsumer<StockNotReservedEventConsumer>(context));
	});
});

builder.Services.AddDbContext<OrderAPIDbContext>(opts => opts.UseSqlServer(builder.Configuration.GetConnectionString("SQLServer")));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapPost("/create-order", async (CreateOrderVM model, OrderAPIDbContext context, IPublishEndpoint publishEndpoint) =>
{
	Order.API.Models.Order order = new()
	{
		// burada GUID tipini string olarak tutmak önemli, client kaynaklı olası problemleri önlemek adına
		BuyerId = Guid.TryParse(model.BuyerId, out Guid _buyerId) ? _buyerId : Guid.NewGuid(),
		OrderItems = model.OrderItems.Select(oi => new OrderItem()
		{
			Quantity = oi.Quantity,
			Price = oi.Price,
			ProductId = Guid.Parse(oi.ProductId)
		}).ToList(),
		OrderStatus = Order.API.Enums.OrderStatus.Suspend,
		CreatedDate = DateTime.UtcNow,
		TotalPrice = model.OrderItems.Sum(oi => oi.Quantity * oi.Price)
	};

	context.Orders.Add(order);
	context.SaveChanges();

	OrderCreatedEvent orderCreatedEvent = new()
	{
		BuyerId = order.BuyerId,
		OrderId = order.Id,
		TotalPrice = order.TotalPrice,
		OrderItems = order.OrderItems.Select(oi => new OrderItemMessage()
		{
			Quantity = oi.Quantity,
			Price = oi.Price,
			ProductId = oi.ProductId
		}).ToList()
	};

	await publishEndpoint.Publish(orderCreatedEvent);
});

app.Run();
