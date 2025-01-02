using CQRS.Example.Manual_CQRS.Queries.Requests;
using CQRS.Example.Manual_CQRS.Queries.Responses;
using CQRS.Example.Models;

namespace CQRS.Example.Manual_CQRS.Handlers.QueryHandlers;

public class GetProductByIdQueryHandler
{
    public GetProductByIdQueryResponse GetByIdProduct(GetProductByIdQueryRequest request)
    {
        var product = ApplicationDbContext.Products.FirstOrDefault(p => p.Id == request.ProductId);
        return new GetProductByIdQueryResponse
        {
            Id = product.Id,
            CreatedDate = product.CreatedDate,
            Name = product.Name,
            Price = product.Price,
            Quantity = product.Quantity,
        };
    }
}