namespace Shared.Messages;

public class OrderItemMessage
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}
