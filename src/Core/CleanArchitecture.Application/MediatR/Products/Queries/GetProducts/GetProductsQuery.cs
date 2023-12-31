﻿using CleanArchitecture.Application.Models;
using MediatR;

namespace CleanArchitecture.Application.MediatR.Products.Queries.GetProducts;

public class GetProductsQuery : IRequest<List<ProductVm>>
{
}