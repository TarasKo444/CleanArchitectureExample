using System.Linq.Expressions;
using CleanArchitecture.Domain.Abstractions.Base;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Persistence.Repositories.Base;

public abstract class BaseRepository<T> : IGenericRepository<T> 
    where T : class
{
    private readonly AppDbContext _context;
    private readonly DbSet<T> _dbSet;

    protected BaseRepository(AppDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task AddAsync(T entity) => 
        await _dbSet.AddAsync(entity);

    public void Remove(T entity) =>
        _dbSet.Remove(entity);

    public void Update(T entity) =>
        _dbSet.Update(entity);

    public async Task<bool> AnyAsync() =>
        await _dbSet.AnyAsync();

    public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate) =>
        await _dbSet.AnyAsync(predicate);

    public async Task<T?> FindAsync(Guid id) =>
        await _dbSet.FindAsync(id);

    public async Task<T?> FirstOrDefaultAsync() =>
        await _dbSet.FirstOrDefaultAsync();

    public async Task<List<T>> ToListAsync() =>
        await _dbSet.ToListAsync();

    public async Task SaveChangesAsync() =>
        await _context.SaveChangesAsync();
}
