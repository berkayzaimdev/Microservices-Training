﻿using MassTransit;
using Shared.PaymentEvents;
using Shared.Settings;

namespace Payment.API.Consumers;

public class PaymentStartedEventConsumer : IConsumer<PaymentStartedEvent>
{
	private readonly ISendEndpointProvider _sendEndpointProvider;

	public PaymentStartedEventConsumer(ISendEndpointProvider sendEndpointProvider)
	{
		_sendEndpointProvider = sendEndpointProvider;
	}

	public async Task Consume(ConsumeContext<PaymentStartedEvent> context)
	{
		var sendEndpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri($"queue:{RabbitMQSettings.StateMachineQueue}"));

		if (new Random().NextInt64(1) != 0)
        {
            PaymentCompletedEvent paymentCompletedEvent = new(context.Message.CorrelationId)
            {

            };

			await sendEndpoint.Send(paymentCompletedEvent);
        }

        else
        {
			PaymentFailedEvent paymentFailedEvent = new(context.Message.CorrelationId)
			{
                Message = "Yetersiz bakiye",
                OrderItems = context.Message.OrderItems // stockrollback'te kullanmamız için bu veriyi taşımamız önemli
			};

			await sendEndpoint.Send(paymentFailedEvent);
		}
	}
}
