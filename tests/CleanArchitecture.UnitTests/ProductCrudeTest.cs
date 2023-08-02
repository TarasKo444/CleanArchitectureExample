using AutoFixture;
using CleanArchitecture.Application.Commands.Products.Commands.CreateProduct;
using CleanArchitecture.Application.Commands.Products.Commands.DeleteProduct;
using CleanArchitecture.Application.Commands.Products.Commands.UpdateProduct;
using CleanArchitecture.Application.Commands.Products.Queries.GetProduct;
using CleanArchitecture.Application.Commands.Products.Queries.GetProducts;
using CleanArchitecture.Common.Exceptions;
using CleanArchitecture.Domain.Abstractions;
using CleanArchitecture.Persistence;
using CleanArchitecture.Persistence.Repositories;
using FluentAssertions;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.EFCoreTests;

public class ProductCrudeTest
{
    private readonly IProductRepository _repository;
    private readonly IMapper _mapper;
    private readonly IFixture _fixture;

    public ProductCrudeTest()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _repository = new ProductRepository(new(options));
        _mapper = new Mapper();
        _fixture = new Fixture();
    }

    #region AddProduct

    [Fact]
    public async Task<Guid> AddProductTest()
    {
        var command = _fixture.Create<CreateProductCommand>();

        var commandHandler = new CreateProductCommandHandler(_mapper, _repository);

        var id = await commandHandler.Handle(command, new());

        (await _repository.ToListAsync()).Should().NotBeEmpty();
        (await _repository.ToListAsync()).Should().HaveCount(1);
        (await _repository.ToListAsync())[0].Id.Should().Be(id);
        return id;
    }

    [Fact]
    public async Task AddExistingProductTest()
    {
        await AddProductTest();

        var product = (await _repository.FirstOrDefaultAsync())!;

        var command = new CreateProductCommand()
        {
            Name = product.Name,
            Description = product.Description,
            Price = product.Price
        };
        var commandHandler = new CreateProductCommandHandler(_mapper, _repository);

        await commandHandler
            .Invoking(h => h.Handle(command, new()))
            .Should()
            .ThrowAsync<UserFriendlyException>()
            .Where(e => e.Status == 403);
    }

    #endregion

    #region DeleteProduct

    [Fact]
    public async Task SuccessDeleteProductTest()
    {
        await AddProductTest();
        var product = (await _repository.FirstOrDefaultAsync())!;

        var command = new DeleteProductCommand()
        {
            Id = product.Id
        };

        var commandHandler = new DeleteProductCommandHandler(_repository);

        await commandHandler.Handle(command, new());

        (await _repository.ToListAsync()).Should().BeEmpty();
    }

    [Fact]
    public async Task UnSuccessDeleteProductTest()
    {
        var command = new DeleteProductCommand()
        {
            Id = Guid.NewGuid()
        };

        var commandHandler = new DeleteProductCommandHandler(_repository);

        await commandHandler
            .Invoking(h => h.Handle(command, new()))
            .Should()
            .ThrowAsync<UserFriendlyException>()
            .Where(e => e.Status == 404);
    }

    #endregion

    #region UpdateProduct

    [Fact]
    public async Task SuccessUpdateProductTest()
    {
        var command = _fixture
            .Build<UpdateProductCommand>()
            .With(c => c.Id, await AddProductTest())
            .Create();

        var handler = new UpdateProductCommandHandler(_repository, _mapper);

        await handler.Handle(command, new());

        (await _repository.ToListAsync()).Should().HaveCount(1);
        (await _repository.FirstOrDefaultAsync())!.Id.Should().Be(command.Id!.Value);
        (await _repository.FirstOrDefaultAsync())!.Name.Should().Be(command.Name);
    }

    [Fact]
    public async Task UpdateProductNameToExistingTest()
    {
        await AddProductTest();
        var product = (await _repository.FirstOrDefaultAsync())!;

        var commandHandler = new CreateProductCommandHandler(_mapper, _repository);
        var addCommand = _fixture.Create<CreateProductCommand>();
        var id = await commandHandler.Handle(addCommand, new());
        var productWithNameToCheck = (await _repository.FindAsync(id))!;

        var command = new UpdateProductCommand()
        {
            Id = product.Id,
            Name = productWithNameToCheck.Name,
            Description = productWithNameToCheck.Description,
            Price = productWithNameToCheck.Price
        };

        var handler = new UpdateProductCommandHandler(_repository, _mapper);

        await handler
            .Invoking(h => h.Handle(command, new()))
            .Should()
            .ThrowAsync<UserFriendlyException>()
            .Where(e => e.Status == 403);
    }

    [Fact]
    public async Task DontUpdateTest()
    {
        await AddProductTest();
        var product = (await _repository.FirstOrDefaultAsync())!;

        var command = new UpdateProductCommand()
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price
        };

        var handler = new UpdateProductCommandHandler(_repository, _mapper);

        await handler
            .Invoking(h => h.Handle(command, new()))
            .Should()
            .NotThrowAsync<UserFriendlyException>();
    }

    [Fact]
    public async Task UnSuccessUpdateProductTest()
    {
        var command = _fixture.Create<UpdateProductCommand>();

        var handler = new UpdateProductCommandHandler(_repository, _mapper);

        await handler
            .Invoking(h => h.Handle(command, new()))
            .Should()
            .ThrowAsync<UserFriendlyException>()
            .Where(e => e.Status == 404);
    }

    #endregion

    #region GetProducts

    [Fact]
    public async Task GetProductsTest()
    {
        var commandHandler = new CreateProductCommandHandler(_mapper, _repository);
        for (int i = 0; i < 3; i++)
        {
            var command = _fixture.Create<CreateProductCommand>();
            await commandHandler.Handle(command, new());
        }

        var query = new GetProductsQuery();
        var handler = new GetProductsQueryHandler(_repository, _mapper);

        (await handler.Handle(query, new())).Should().HaveCount(3);
    }

    [Fact]
    public async Task GetEmptyProductsTest()
    {
        var command = new DeleteProductCommand()
        {
            Id = await AddProductTest()
        };

        var commandHandler = new DeleteProductCommandHandler(_repository);
        await commandHandler.Handle(command, new());

        var query = new GetProductsQuery();
        var handler = new GetProductsQueryHandler(_repository, _mapper);

        (await handler.Handle(query, new())).Should().BeEmpty();
    }

    [Fact]
    public async Task GetOneProduct()
    {
        var createCommand = _fixture.Create<CreateProductCommand>();
        var createCommandHandler = new CreateProductCommandHandler(_mapper, _repository);
        var id = await createCommandHandler.Handle(createCommand, new());

        var query = new GetProductQuery() { Id = id };
        var handler = new GetProductQueryHandler(_repository, _mapper);

        (await handler.Handle(query, new())).Id.Should().Be(id);
        (await _repository.ToListAsync()).Should().HaveCount(1);
    }

    #endregion
}