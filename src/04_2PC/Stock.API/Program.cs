var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/ready", () =>
{
	Console.WriteLine("stock service is ready");
	return true;
});

app.MapGet("/commit", () =>
{
	Console.WriteLine("stock service is moited");
	return true;
});

app.MapGet("/rollback", () =>
{
	Console.WriteLine("stock service is rolled back");
	return true;
});

app.Run();
