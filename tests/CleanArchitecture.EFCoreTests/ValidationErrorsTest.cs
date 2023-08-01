using AutoFixture;
using CleanArchitecture.Application.Commands.Products.Commands.CreateProduct;
using CleanArchitecture.Application.Commands.Products.Commands.DeleteProduct;
using CleanArchitecture.Domain.Abstractions;
using CleanArchitecture.Persistence;
using CleanArchitecture.Persistence.Repositories;
using FluentAssertions;
using FluentValidation;
using MapsterMapper;
using MediatR;
using MediatR.Extensions.FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.EFCoreTests;

public class ValidationErrorsTest
{
    private readonly IProductRepository _repository;
    private readonly IMapper _mapper;
    private readonly IFixture _fixture;

    public ValidationErrorsTest()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _repository = new ProductRepository(new(options));
        _mapper = new Mapper();
        _fixture = new Fixture();
    }

    [Fact]
    public async Task NegativePriceTest()
    {
        var command = _fixture
            .Build<CreateProductCommand>()
            .With(p => p.Price, -1)
            .Create();

        var validationBehavior = new ValidationBehavior<CreateProductCommand, Guid>(new[]
        {
            new CreateProductCommandValidator()
        });

        await validationBehavior
            .Invoking(v => v.Handle(command, null!, new()))
            .Should()
            .ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task EmptyDescriptionTest()
    {
        var command = _fixture
            .Build<CreateProductCommand>()
            .With(p => p.Description, string.Empty)
            .Create();

        var validationBehavior = new ValidationBehavior<CreateProductCommand, Guid>(new[]
        {
            new CreateProductCommandValidator()
        });

        await validationBehavior
            .Invoking(v => v.Handle(command, null!, new()))
            .Should()
            .ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task DeleteEmptyGuidTest()
    {
        var createCommand = _fixture.Create<CreateProductCommand>();
        var handler = new CreateProductCommandHandler(_mapper, _repository);
        await handler.Handle(createCommand, new());

        var validationBehavior = new ValidationBehavior<DeleteProductCommand, Unit>(new[]
        {
            new DeleteProductCommandValidator()
        });

        var command = new DeleteProductCommand()
        {
            Id = Guid.Empty
        };

        await validationBehavior
            .Invoking(v => v.Handle(command, null!, new()))
            .Should()
            .ThrowAsync<ValidationException>();
    }
}