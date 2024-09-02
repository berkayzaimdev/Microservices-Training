using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using ServiceB.Models.Entities;
using ServiceB.Services;
using System;

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

app.MapGet("update/{personId}/{newName}", async (
	[FromRoute] string personId,
	[FromRoute] string newName,
	MongoDBService mongoDBService) =>
{
	var employees = mongoDBService.GetCollection<Employee>();
	Employee employee = await (await employees.FindAsync(e => e.PersonId == personId)).FirstOrDefaultAsync();
	employee.Name = newName;
	await employees.FindOneAndReplaceAsync(p => p.Id == employee.Id, employee);
	return true;
});

await app.RunAsync();
