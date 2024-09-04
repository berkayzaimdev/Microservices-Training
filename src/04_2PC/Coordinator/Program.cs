using Coordinator.Models.Contexts;
using Coordinator.Services;
using Coordinator.Services.Abstractions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TwoPhaseCommitContext>(opts =>
{
	opts.UseSqlServer(builder.Configuration.GetConnectionString("SQLServer"));
});

builder.Services.AddHttpClient("OrderAPI", client => client.BaseAddress = new("https://localhost:7007/"));
builder.Services.AddHttpClient("StockAPI", client => client.BaseAddress = new("https://localhost:7273/"));
builder.Services.AddHttpClient("PaymentAPI", client => client.BaseAddress = new("https://localhost:7012/"));
// erişeceğimiz servisleri client olarak coordinator projesine ekledik

builder.Services.AddSingleton<ITransactionService, TransactionService>();

var app = builder.Build();

app.MapGet("/create-order-transaction", async (ITransactionService transactionService) =>
{
	// Prepare Phase
	var transactionId = await transactionService.CreateTransaction();
	await transactionService.PrepareServices(transactionId);
	bool transactionState = await transactionService.CheckReadyServices(transactionId);

	if (transactionState)
	{
		// Commit Phase
		await transactionService.Commit(transactionId);
		transactionState = await transactionService.CheckTransactionStateServices(transactionId);
	}

    if (!transactionState)
    {
		await transactionService.Rollback(transactionId);
    }
});

await app.RunAsync();
