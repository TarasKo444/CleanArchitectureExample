using MediatR;

namespace CleanArchitecture.Application.Commands.Products.Commands.DeleteProduct;

public class DeleteProductCommand : IRequest<Unit>
{
    public Guid Id { get; set; }
}
