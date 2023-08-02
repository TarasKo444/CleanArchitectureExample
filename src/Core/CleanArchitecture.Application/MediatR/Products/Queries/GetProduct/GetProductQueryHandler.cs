using CleanArchitecture.Application.Models;
using CleanArchitecture.Common.Exceptions;
using CleanArchitecture.Domain.Abstractions;
using MapsterMapper;
using MediatR;

namespace CleanArchitecture.Application.MediatR.Products.Queries.GetProduct;

public class GetProductQueryHandler : IRequestHandler<GetProductQuery, ProductVm>
{
    private readonly IMapper _mapper;
    private readonly IProductRepository _repository;

    public GetProductQueryHandler(IMapper mapper, IProductRepository repository)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ProductVm> Handle(GetProductQuery request, CancellationToken cancellationToken)
    {
        var product = await _repository.FindAsync(request.Id);
        
        if (product is null)
            throw new UserFriendlyException(404, "Product with given id not found");

        return _mapper.Map<ProductVm>(product);
    }
}
