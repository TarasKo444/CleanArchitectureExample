using CleanArchitecture.Application.Models;
using MediatR;

namespace CleanArchitecture.Application.Commands.Products.Queries.GetProduct;

public class GetProductQuery : IRequest<ProductVm>
{
    public Guid Id { get; set; }
}
