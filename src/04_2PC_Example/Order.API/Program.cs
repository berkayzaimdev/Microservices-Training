var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/ready", () =>
{
	Console.WriteLine("order service is ready");
	return true;
});

app.MapGet("/commit", () =>
{
	Console.WriteLine("order service is moited");
	return true;
});

app.MapGet("/rollback", () =>
{
	Console.WriteLine("order service is rolled back");
	return true;
});

app.Run();
