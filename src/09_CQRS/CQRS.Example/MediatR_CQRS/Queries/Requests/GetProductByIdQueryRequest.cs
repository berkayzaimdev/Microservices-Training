using CQRS.Example.MediatR_CQRS.Queries.Responses;
using MediatR;

namespace CQRS.Example.MediatR_CQRS.Queries.Requests;

public class GetProductByIdQueryRequest : IRequest<GetProductByIdQueryResponse>
{
    public Guid ProductId { get; set; }
}