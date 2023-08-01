using CleanArchitecture.Common;
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
        
        Throw.UserFriendlyExceptionIfNull(product,
            404, "Product with given id not found");

        Throw.UserFriendlyExceptionIf(
            await _repository.AnyAsync(p => p.Id != command.Id && p.Name == command.Name),
            403, "Product already exist");
        
        _mapper.Map(command, product);
        _repository.Update(product!);
        await _repository.SaveChangesAsync();
    }
}
