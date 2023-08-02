using FluentValidation;

namespace CleanArchitecture.Application.Commands.Products.Commands.DeleteProduct;

public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
{
    public DeleteProductCommandValidator()
    {
        RuleFor(p => p.Id)
            .NotEqual(Guid.Empty).WithMessage("Id cannot be empty");
    }
}
