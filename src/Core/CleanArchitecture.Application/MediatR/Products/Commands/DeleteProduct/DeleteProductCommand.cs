using MediatR;

namespace CleanArchitecture.Application.MediatR.Products.Commands.DeleteProduct;

public class DeleteProductCommand : IRequest<Unit>
{
    public Guid Id { get; set; }
}
