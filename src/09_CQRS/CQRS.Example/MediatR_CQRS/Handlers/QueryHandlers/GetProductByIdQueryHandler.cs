using CQRS.Example.MediatR_CQRS.Queries.Requests;
using CQRS.Example.MediatR_CQRS.Queries.Responses;
using CQRS.Example.Models;
using MediatR;

namespace CQRS.Example.MediatR_CQRS.Handlers.QueryHandlers;

public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQueryRequest, GetProductByIdQueryResponse>
{
    public async Task<GetProductByIdQueryResponse> Handle(GetProductByIdQueryRequest request, CancellationToken cancellationToken)
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