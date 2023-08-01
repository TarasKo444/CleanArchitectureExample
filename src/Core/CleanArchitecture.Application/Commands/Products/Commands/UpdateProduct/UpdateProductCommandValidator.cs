using FluentValidation;

namespace CleanArchitecture.Application.Commands.Products.Commands.UpdateProduct;

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEqual(Guid.Empty);

        RuleFor(x => x.Name)
            .NotNull().WithMessage("Product name cannot be null")
            .NotEmpty().WithMessage("Product name cannot be empty");

        RuleFor(x => x.Description)
            .NotNull().WithMessage("Product description cannot be null")
            .NotEmpty().WithMessage("Product description cannot be empty")
            .MaximumLength(250).WithMessage("Product description length should be 250 or less");

        RuleFor(x => x.Price)
            .GreaterThanOrEqualTo(0).WithMessage("Product price should be positive number");
    }
}
