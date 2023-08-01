using MediatR;

namespace CleanArchitecture.Application.Commands.Products.Commands.UpdateProduct;

public class UpdateProductCommand : IRequest
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
}
