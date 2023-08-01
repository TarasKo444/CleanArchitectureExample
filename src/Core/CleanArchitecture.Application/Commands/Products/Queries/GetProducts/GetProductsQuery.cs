using CleanArchitecture.Application.Models;
using MediatR;

namespace CleanArchitecture.Application.Commands.Products.Queries.GetProducts;

public class GetProductsQuery : IRequest<List<ProductVm>>
{
}