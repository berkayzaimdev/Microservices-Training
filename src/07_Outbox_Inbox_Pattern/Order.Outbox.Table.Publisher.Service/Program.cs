using MassTransit;
using Order.Outbox.Table.Publisher.Service.Jobs;
using Quartz;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddMassTransit(configurator =>
{
	configurator.UsingRabbitMq((context, _configure) =>
	{
		_configure.Host("...");
	});
});

builder.Services.AddQuartz(configurator =>
{
	JobKey jobKey = new JobKey("OrderOutboxPublishJob");

	configurator.AddJob<OrderOutboxPublishJob>(opts => opts.WithIdentity(jobKey));

	TriggerKey triggerKey = new TriggerKey("OrderOutboxPublishTrigger");

	configurator.AddTrigger(opts => opts.ForJob(jobKey)
									 .WithIdentity(triggerKey)
									 .StartAt(DateTime.UtcNow)
									 .WithSimpleSchedule(builder => builder.WithIntervalInSeconds(5).RepeatForever()));
});

builder.Services.AddQuartzHostedService(opts => opts.WaitForJobsToComplete = true);

var host = builder.Build();
host.Run();
