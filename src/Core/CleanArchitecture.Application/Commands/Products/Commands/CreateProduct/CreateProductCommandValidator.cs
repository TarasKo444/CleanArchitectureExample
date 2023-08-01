using FluentValidation;

namespace CleanArchitecture.Application.Commands.Products.Commands.CreateProduct;

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Product description cannot be empty.")
            .MaximumLength(250).WithMessage("Product description length should be 250 or less");

        RuleFor(x => x.Price)
            .GreaterThanOrEqualTo(0).WithMessage("Product price should be positive number");
    }
}
