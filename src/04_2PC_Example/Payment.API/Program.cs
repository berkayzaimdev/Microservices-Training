var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/ready", () =>
{
	Console.WriteLine("payment service is ready");
	return true;
});

app.MapGet("/commit", () =>
{
	Console.WriteLine("payment service is moited");
	return true;
});

app.MapGet("/rollback", () =>
{
	Console.WriteLine("payment service is rolled back");
	return true;
});

app.Run();
