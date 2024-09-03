using MassTransit;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using ServiceA.Models.Entities;
using ServiceA.Services;
using Shared.Events;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<MongoDBService>();
builder.Services.AddMassTransit(configurator =>
{
	configurator.UsingRabbitMq((context, _configurator) =>
	{
		_configurator.Host("...");
	});
});

using IServiceScope scope = builder.Services.BuildServiceProvider().CreateScope();
var mongoDBService = scope.ServiceProvider.GetService<MongoDBService>();
var collection = mongoDBService.GetCollection<Person>();
if (!collection.FindSync(s => true).Any())
{
	await collection.InsertOneAsync(new() { Name = "Gençay" });
	await collection.InsertOneAsync(new() { Name = "Hilmi" });
	await collection.InsertOneAsync(new() { Name = "Şuayip" });
	await collection.InsertOneAsync(new() { Name = "Rakıf" });
	await collection.InsertOneAsync(new() { Name = "Rıfkı" });
	await collection.InsertOneAsync(new() { Name = "Muiddin" });
}

var app = builder.Build();

app.MapGet("/{id}/{newName}", async (
	[FromRoute] string id,
	[FromRoute] string newName,
	MongoDBService mongoDBService,
	IPublishEndpoint publishEndpoint) =>
{
	var persons = mongoDBService.GetCollection<Person>();

	Person person = await (await persons.FindAsync(person => person.Id == ObjectId.Parse(id))).FirstOrDefaultAsync();
	person.Name = newName;
	await persons.FindOneAndReplaceAsync(p => p.Id == ObjectId.Parse(id), person);

	UpdatedPersonNameEvent updatedPersonNameEvent = new()
	{
		PersonId = id,
		NewName = newName
	};

	await publishEndpoint.Publish(updatedPersonNameEvent);
});

await app.RunAsync();
