using CQRS.Example.Manual_CQRS.Queries.Requests;
using CQRS.Example.Manual_CQRS.Queries.Responses;
using CQRS.Example.Models;

namespace CQRS.Example.Manual_CQRS.Handlers.QueryHandlers;

public class GetAllProductQueryHandler
{
    public List<GetAllProductsQueryResponse> GetAllProduct(GetAllProductsQueryRequest request)
    {
        return ApplicationDbContext.Products.Select(p => new GetAllProductsQueryResponse
        {
            Id = p.Id,
            CreatedDate = p.CreatedDate,
            Name = p.Name,
            Price = p.Price,
            Quantity = p.Quantity,
        }).ToList();
    }
}