using CleanArchitecture.Common;
using CleanArchitecture.Domain.Abstractions;
using CleanArchitecture.Domain.Entities;
using MapsterMapper;
using MediatR;

namespace CleanArchitecture.Application.MediatR.Products.Commands.CreateProduct;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Guid>
{
    private readonly IMapper _mapper;
    private readonly IProductRepository _repository;

    public CreateProductCommandHandler(IMapper mapper, IProductRepository repository)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Guid> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        command.Name = command.Name!.Trim();
        command.Description = command.Description!.Trim();
        
        var product = _mapper.Map<Product>(command);

        Throw.UserFriendlyExceptionIf(
            await _repository.AnyAsync(p => p.Name == product.Name),
            403, "Product already exist");
        
        await _repository.AddAsync(product);
        await _repository.SaveChangesAsync();
        
        return product.Id;
    }
}