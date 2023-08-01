using CleanArchitecture.Application.Models;
using CleanArchitecture.Domain.Abstractions;
using MapsterMapper;
using MediatR;

namespace CleanArchitecture.Application.Commands.Products.Queries.GetProducts;

public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, List<ProductVm>>
{
    private readonly IProductRepository _repository;
    private readonly IMapper _mapper;

    public GetProductsQueryHandler(IProductRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<List<ProductVm>> Handle(GetProductsQuery query, CancellationToken cancellationToken)
    {
        return (await _repository
            .ToListAsync())
            .Select(t => _mapper.Map<ProductVm>(t))
            .ToList();
    }
}
