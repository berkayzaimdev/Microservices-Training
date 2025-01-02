using CQRS.Example.MediatR_CQRS.Commands.Responses;
using MediatR;

namespace CQRS.Example.MediatR_CQRS.Commands.Requests;

public class CreateProductCommandRequest : IRequest<CreateProductCommandResponse>
{
    public string Name { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}