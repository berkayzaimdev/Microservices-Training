using Order.API.Enums;
using Order.API.Models;

namespace Order.API.ViewModels;

public class CreateOrderVM
{
	public int BuyerId { get; set; }
	public ICollection<OrderItemVM> OrderItems { get; set; }
}

public class OrderItemVM
{
	public int ProductId { get; set; }
	public int Count { get; set; }
	public decimal Price { get; set; }
}
