using CleanArchitecture.Common;
using CleanArchitecture.Common.Exceptions;
using CleanArchitecture.Domain.Abstractions;
using MediatR;

namespace CleanArchitecture.Application.Commands.Products.Commands.DeleteProduct;

public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, Unit>
{
    private readonly IProductRepository _repository;

    public DeleteProductCommandHandler(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<Unit> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
    {
        var product = await _repository.FindAsync(command.Id);
        
        if (product is null)
            throw new UserFriendlyException(404, "Product with given id not found");

        _repository.Remove(product);
        await _repository.SaveChangesAsync();
        return Unit.Value;
    }
}
