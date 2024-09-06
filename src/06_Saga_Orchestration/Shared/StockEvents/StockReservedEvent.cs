using MassTransit;
using Shared.Messages;

namespace Shared.StockEvents;

public class StockReservedEvent : CorrelatedBy<Guid>
{
    // public int OrderId { get; set; } // merkezi bir yapı kurduğumuz için, bu bağımlılığa ihtiyacımız yok
    public StockReservedEvent(Guid correlationId)
    {
        CorrelationId = correlationId;
    }

    public Guid CorrelationId { get; }
    public List<OrderItemMessage> OrderItems { get; set; }
}
