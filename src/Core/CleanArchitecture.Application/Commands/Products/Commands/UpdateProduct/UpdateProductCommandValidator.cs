using FluentValidation;

namespace CleanArchitecture.Application.Commands.Products.Commands.UpdateProduct;

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(p => p.Id)
            .NotEqual(Guid.Empty);
        
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Product description cannot be empty.")
            .MaximumLength(250).WithMessage("Product description length should be 250 or less");

        RuleFor(x => x.Price)
            .GreaterThanOrEqualTo(0).WithMessage("Product price should be positive number");
    }
}
