using CQRS.Example.Manual_CQRS.Commands.Requests;
using CQRS.Example.Manual_CQRS.Commands.Responses;
using CQRS.Example.Models;

namespace CQRS.Example.Manual_CQRS.Handlers.CommandHandlers;

public class DeleteProductCommandHandler
{
    public DeleteProductCommandResponse DeleteProduct(DeleteProductCommandRequest request)
    {
        var guid = Guid.NewGuid();

        ApplicationDbContext.Products.Remove(ApplicationDbContext.Products.Single(x => x.Id == request.ProductId));

        return new DeleteProductCommandResponse()
        {
            IsSuccess = true
        };
    }
}