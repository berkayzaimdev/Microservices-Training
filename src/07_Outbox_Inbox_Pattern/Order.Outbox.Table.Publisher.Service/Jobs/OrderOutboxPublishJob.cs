using MassTransit;
using Order.Outbox.Table.Publisher.Service.Entities;
using Quartz;
using Shared.Events;
using System.Text.Json;

namespace Order.Outbox.Table.Publisher.Service.Jobs;

public class OrderOutboxPublishJob(IPublishEndpoint publishEndpoint) : IJob
{
	public async Task Execute(IJobExecutionContext context)
	{
		if(OrderOutboxSingletonDatabase.DataReaderState)
		{
			OrderOutboxSingletonDatabase.DataReaderBusy(); // şuanda işlem yaptığımız için, db'yi meşgul duruma çek

			List<OrderOutbox> orderOutboxes = (await OrderOutboxSingletonDatabase.QueryAsync<OrderOutbox>
				($@"SELECT * FROM OrderOutboxes 
					WHERE PROCESSEDDATE IS NULL 
					ORDER BY OCCUREDON ASC")).ToList();

            foreach (var orderOutbox in orderOutboxes)
            {
                if (orderOutbox.Type == nameof(OrderCreatedEvent))
                {
					OrderCreatedEvent? orderCreatedEvent = JsonSerializer.Deserialize<OrderCreatedEvent>(orderOutbox.Payload);

                    if (orderCreatedEvent != null)
                    {
						await publishEndpoint.Publish(orderCreatedEvent);
						await OrderOutboxSingletonDatabase.ExecuteAsync
							($@"UPDATE OrderOutboxes 
								SET PROCESSEDDATE = GETDATE() 
								WHERE ID = '{orderOutbox.Id}'");
                    }
                }
            }

			OrderOutboxSingletonDatabase.DataReaderReady(); // işlem tamamlandığı için db'yi hazır duruma getirdik
			await Console.Out.WriteLineAsync("OrderOutbox table checked!");
        }
	}
}
