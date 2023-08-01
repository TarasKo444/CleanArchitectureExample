using CleanArchitecture.Common.Exceptions;
using CleanArchitecture.Domain.Abstractions;
using MapsterMapper;
using MediatR;

namespace CleanArchitecture.Application.Commands.Products.Commands.UpdateProduct;

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand>
{
    private readonly IProductRepository _repository;
    private readonly IMapper _mapper;

    public UpdateProductCommandHandler(IProductRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        command.Name = command.Name!.Trim();
        command.Description = command.Description!.Trim();
        
        var product = await _repository.FindAsync(command.Id);
        
        if (product is null)
            throw new UserFriendlyException(404, "Product with given id not found");

        if (await _repository.AnyAsync(p => p.Id != command.Id && p.Name == command.Name))
            throw new UserFriendlyException(403, "Product already exist");
        
        _mapper.Map(command, product);
        _repository.Update(product);
        await _repository.SaveChangesAsync();
    }
}
