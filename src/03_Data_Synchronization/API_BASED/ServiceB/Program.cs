using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using ServiceA.Models.Entities;
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
var collection = mongoDBService.GetCollection<Employee>();
if (!collection.FindSync(s => true).Any())
{
	await collection.InsertOneAsync(new() { Name = "Gençay", PersonId= "asdasd", Department = "Yazılım" });
	await collection.InsertOneAsync(new() { Name = "Hilmi", PersonId = "asdasd", Department = "Elektrik" });
	await collection.InsertOneAsync(new() { Name = "Şuayip", PersonId = "asdasd", Department = "Elektronik" });
	await collection.InsertOneAsync(new() { Name = "Rakıf", PersonId = "asdasd", Department = "Muhasebe" });
	await collection.InsertOneAsync(new() { Name = "Rıfkı", PersonId = "asdasd", Department = "Şoför" });
	await collection.InsertOneAsync(new() { Name = "Muiddin", PersonId = "asdasd", Department = "İnsan Kaynakları" });
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
