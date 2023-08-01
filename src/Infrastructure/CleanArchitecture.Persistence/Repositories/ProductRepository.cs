using CleanArchitecture.Domain.Abstractions;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Persistence.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Persistence.Repositories;

public class ProductRepository : BaseRepository<Product>, IProductRepository
{
    public ProductRepository(AppDbContext context) : base(context)
    {
    }
}
