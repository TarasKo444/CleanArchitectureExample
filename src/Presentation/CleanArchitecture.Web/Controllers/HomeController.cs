using CleanArchitecture.Application.MediatR.Products.Commands.CreateProduct;
using CleanArchitecture.Application.MediatR.Products.Commands.DeleteProduct;
using CleanArchitecture.Application.MediatR.Products.Commands.UpdateProduct;
using CleanArchitecture.Application.MediatR.Products.Queries.GetProduct;
using CleanArchitecture.Application.MediatR.Products.Queries.GetProducts;
using CleanArchitecture.Application.Models;
using CleanArchitecture.Web.Models;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.Web.Controllers;

[ApiController]
[Route("/")]
public class HomeController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public HomeController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<List<ProductVm>> GetAll()
    {
        var query = new GetProductsQuery();
        return await _mediator.Send(query);
    }

    [HttpGet("{guid:guid}")]
    public async Task<ProductVm> Get(Guid guid)
    {
        var query = new GetProductQuery { Id = guid };
        return await _mediator.Send(query);
    }

    [HttpPost]
    public async Task<Guid> Add([FromBody] CreateProductDto productDto)
    {
        var command = _mapper.Map<CreateProductCommand>(productDto);
        return await _mediator.Send(command);
    }

    [HttpDelete("{guid:guid}")]
    public async Task Delete(Guid guid)
    {
        var command = new DeleteProductCommand { Id = guid };
        await _mediator.Send(command);
    }

    [HttpPut]
    public async Task Update([FromBody] UpdateProductDto productDto)
    {
        var command = _mapper.Map<UpdateProductCommand>(productDto);
        await _mediator.Send(command);
    }
}