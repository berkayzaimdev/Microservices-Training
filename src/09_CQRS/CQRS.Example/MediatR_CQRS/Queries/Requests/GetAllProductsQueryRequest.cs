using CQRS.Example.MediatR_CQRS.Queries.Responses;
using MediatR;

namespace CQRS.Example.MediatR_CQRS.Queries.Requests;

public class GetAllProductsQueryRequest : IRequest<List<GetAllProductsQueryResponse>>
{
    
}