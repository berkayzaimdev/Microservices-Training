using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SagaStateMachine.Service.StateInstances;

namespace SagaStateMachine.Service.StateMaps;

public class OrderStateMap : SagaClassMap<OrderStateInstance>
{
	// state instance ile ilgili validasyon işlemlerini yaptığımız class
	protected override void Configure(EntityTypeBuilder<OrderStateInstance> entity, ModelBuilder model)
	{
		entity.Property(x => x.BuyerId).IsRequired();

		entity.Property(x => x.OrderId).IsRequired();

		entity.Property(x => x.TotalPrice).HasDefaultValue(Decimal.Zero);
	}
}
