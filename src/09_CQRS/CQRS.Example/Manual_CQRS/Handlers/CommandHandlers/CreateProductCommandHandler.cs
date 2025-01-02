using CQRS.Example.Manual_CQRS.Commands.Requests;
using CQRS.Example.Manual_CQRS.Commands.Responses;
using CQRS.Example.Models;

namespace CQRS.Example.Manual_CQRS.Handlers.CommandHandlers;

public class CreateProductCommandHandler
{
    public CreateProductCommandResponse CreateProduct(CreateProductCommandRequest request)
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