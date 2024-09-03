
using ServiceB.Models.Entities;
using ServiceB.Services;
using MongoDB.Driver;
using Microsoft.AspNetCore.Mvc;
using MassTransit;
using ServiceB.Consumers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSingleton<MongoDBService>();
builder.Services.AddMassTransit(configurator =>
{
	configurator.UsingRabbitMq((context, _configurator) =>
	{
		_configurator.Host("...");
		_configurator.ReceiveEndpoint("update_person_name_queue", e => 
		e.ConfigureConsumer<UpdatePersonNameEventConsumer>(context));
	});
});

using IServiceScope scope = builder.Services.BuildServiceProvider().CreateScope();
var mongoDBService = scope.ServiceProvider.GetService<MongoDBService>();
var collection = mongoDBService.GetCollection<Employee>();
if (!collection.FindSync(s => true).Any())
{
	await collection.InsertOneAsync(new() { Name = "Gençay", PersonId = "asdasd", Department = "Yazılım" });
	await collection.InsertOneAsync(new() { Name = "Hilmi", PersonId = "asdasd", Department = "Elektrik" });
	await collection.InsertOneAsync(new() { Name = "Şuayip", PersonId = "asdasd", Department = "Elektronik" });
	await collection.InsertOneAsync(new() { Name = "Rakıf", PersonId = "asdasd", Department = "Muhasebe" });
	await collection.InsertOneAsync(new() { Name = "Rıfkı", PersonId = "asdasd", Department = "Şoför" });
	await collection.InsertOneAsync(new() { Name = "Muiddin", PersonId = "asdasd", Department = "İnsan Kaynakları" });
}

var app = builder.Build();

await app.RunAsync();
