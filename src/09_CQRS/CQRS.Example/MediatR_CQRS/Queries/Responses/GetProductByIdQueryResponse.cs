namespace CQRS.Example.MediatR_CQRS.Queries.Responses;

public class GetProductByIdQueryResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public DateTime CreatedDate { get; set; }
}