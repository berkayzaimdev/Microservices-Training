using CQRS.Example.MediatR_CQRS.Queries.Requests;
using CQRS.Example.MediatR_CQRS.Queries.Responses;
using CQRS.Example.Models;
using MediatR;


namespace CQRS.Example.MediatR_CQRS.Handlers.QueryHandlers;

public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQueryRequest, List<GetAllProductsQueryResponse>>
{
    public async Task<List<GetAllProductsQueryResponse>> Handle(GetAllProductsQueryRequest request,
        CancellationToken cancellationToken)
        =>
            ApplicationDbContext.Products.Select(p => new GetAllProductsQueryResponse
            {
                Id = p.Id,
                CreatedDate = p.CreatedDate,
                Name = p.Name,
                Price = p.Price,
                Quantity = p.Quantity,
            }).ToList();
}