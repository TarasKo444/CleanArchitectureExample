using FluentValidation;

namespace CleanArchitecture.Application.MediatR.Products.Queries.GetProduct;

public class GetProductQueryValidator : AbstractValidator<GetProductQuery>
{
    public GetProductQueryValidator()
    {
        RuleFor(p => p.Id)
            .NotEqual(Guid.Empty).WithMessage("Id cannot be empty");    
    }
}
