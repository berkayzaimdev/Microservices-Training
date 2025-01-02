namespace CQRS.Example.Manual_CQRS.Queries.Requests;

public class GetProductByIdQueryRequest
{
    public Guid ProductId { get; set; }
}