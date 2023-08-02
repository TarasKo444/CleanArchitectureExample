using FluentValidation;

namespace CleanArchitecture.Application.Commands.Products.Queries.GetProduct;

public class GetProductQueryValidator : AbstractValidator<GetProductQuery>
{
    public GetProductQueryValidator()
    {
        RuleFor(p => p.Id)
            .NotEqual(Guid.Empty).WithMessage("Id cannot be empty");    
    }
}
