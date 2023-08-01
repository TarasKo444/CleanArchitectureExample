using CleanArchitecture.Common;
using CleanArchitecture.Common.Exceptions;
using CleanArchitecture.Domain.Abstractions;
using CleanArchitecture.Domain.Entities;
using MapsterMapper;
using MediatR;

namespace CleanArchitecture.Application.Commands.Products.Commands.CreateProduct;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Guid>
{
    private readonly IMapper _mapper;
    private readonly IProductRepository _repository;

    public CreateProductCommandHandler(IMapper mapper, IProductRepository repository)
    {
        _mapper = mapper;
        _repository = repository;
    }

    public async Task<Guid> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        var product = _mapper.Map<Product>(command);
        
        if (await _repository.AnyAsync(p => p.Name == product.Name))
            throw new UserFriendlyException(403, "Product already exist");
        
        await _repository.AddAsync(product);
        await _repository.SaveChangesAsync();
        return product.Id;
    }
}