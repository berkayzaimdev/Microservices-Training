using CQRS.Example.MediatR_CQRS.Commands.Requests;
using CQRS.Example.MediatR_CQRS.Commands.Responses;
using CQRS.Example.Models;
using MediatR;

namespace CQRS.Example.MediatR_CQRS.Handlers.CommandHandlers;

public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommandRequest, DeleteProductCommandResponse>
{
    public async Task<DeleteProductCommandResponse> Handle(DeleteProductCommandRequest request, CancellationToken cancellationToken)
    {
        var guid = Guid.NewGuid();

        ApplicationDbContext.Products.Remove(ApplicationDbContext.Products.Single(x => x.Id == request.ProductId));

        return new DeleteProductCommandResponse()
        {
            IsSuccess = true
        };
    }
}