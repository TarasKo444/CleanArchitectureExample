using FluentValidation;

namespace CleanArchitecture.Application.MediatR.Products.Commands.CreateProduct;

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotNull().WithMessage("Product name not provided")
            .NotEmpty().WithMessage("Product name cannot be empty");
        
        RuleFor(x => x.Description)
            .NotNull().WithMessage("Product description not provided")
            .NotEmpty().WithMessage("Product description cannot be empty")
            .MaximumLength(250).WithMessage("Product description length should be 250 or less");

        RuleFor(x => x.Price)
            .NotNull().WithMessage("Product price not provided")
            .GreaterThanOrEqualTo(0).WithMessage("Product price should be positive number");
    }
}
