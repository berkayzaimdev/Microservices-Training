using CQRS.Example.Manual_CQRS.Handlers.CommandHandlers;
using CQRS.Example.Manual_CQRS.Handlers.QueryHandlers;
using CQRS.Example.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddControllers();

builder.Services
    .AddSingleton<CreateProductCommandHandler>()
    .AddSingleton<DeleteProductCommandHandler>()
    .AddSingleton<GetAllProductQueryHandler>()
    .AddSingleton<GetProductByIdQueryHandler>();

builder.Services.AddMediatR(conf => conf.RegisterServicesFromAssembly(typeof(ApplicationDbContext).Assembly));
    

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();


app.Run();
