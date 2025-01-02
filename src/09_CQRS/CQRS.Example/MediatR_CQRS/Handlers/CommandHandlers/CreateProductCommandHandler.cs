using CQRS.Example.MediatR_CQRS.Commands.Requests;
using CQRS.Example.MediatR_CQRS.Commands.Responses;
using CQRS.Example.Models;
using MediatR;

namespace CQRS.Example.MediatR_CQRS.Handlers.CommandHandlers;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommandRequest, CreateProductCommandResponse>
{
    public async Task<CreateProductCommandResponse> Handle(CreateProductCommandRequest request, CancellationToken cancellationToken)
    {
        var guid = Guid.NewGuid();
        
        ApplicationDbContext.Products.Add(new ()
        {
            Id = guid,
            Name = request.Name,
            Price = request.Price,
            Quantity = request.Quantity,
            CreatedDate = DateTime.UtcNow
        });

        return new CreateProductCommandResponse()
        {
            IsSuccess = true,
            ProductId = guid
        };
    }
}