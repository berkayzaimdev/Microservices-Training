using MassTransit;
using Microsoft.EntityFrameworkCore;
using SagaStateMachine.Service.StateDbContext;
using SagaStateMachine.Service.StateInstances;
using SagaStateMachine.Service.StateMachine;
using Shared.Settings;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddMassTransit(configurator =>
{
	configurator.AddSagaStateMachine<OrderStateMachine, OrderStateInstance>()
	.EntityFrameworkRepository(opts =>
	{
		opts.AddDbContext<DbContext, OrderStateDbContext>((provider, _builder) =>
		{
			_builder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Database=SagaOrchestration;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
		});
	});

	configurator.UsingRabbitMq((context, _configure) =>
	{
		_configure.Host("...");

		_configure.ReceiveEndpoint(RabbitMQSettings.StateMachineQueue, e => e.ConfigureSaga<OrderStateInstance>(context));
	});
});

var host = builder.Build();

host.Run();
