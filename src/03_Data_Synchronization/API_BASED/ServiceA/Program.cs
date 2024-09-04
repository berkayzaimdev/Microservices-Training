using ServiceA.Models.Entities;
using ServiceA.Services;
using MongoDB.Driver;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<MongoDBService>();
builder.Services.AddHttpClient("ServiceB", httpClient =>
{
	httpClient.BaseAddress = new Uri("https://localhost:7009/");
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
	IHttpClientFactory httpClientFactory) =>
{
	var httpClient = httpClientFactory.CreateClient("ServiceB");

	var persons = mongoDBService.GetCollection<Person>();

	Person person = await (await persons.FindAsync(person => person.Id == ObjectId.Parse(id))).FirstOrDefaultAsync();
	person.Name = newName;
	await persons.FindOneAndReplaceAsync(p => p.Id == ObjectId.Parse(id), person);

	var httpResponseMessage = await httpClient.GetAsync($"update/{person.Id}/{person.Name}");
	if (httpResponseMessage.IsSuccessStatusCode)
	{
		var content = await httpResponseMessage.Content.ReadAsStringAsync();
		await Console.Out.WriteLineAsync(content);
	}
	return true;
});

await app.RunAsync();
