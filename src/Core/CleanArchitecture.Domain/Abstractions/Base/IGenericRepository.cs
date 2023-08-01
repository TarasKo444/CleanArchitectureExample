using System.Linq.Expressions;

namespace CleanArchitecture.Domain.Abstractions.Base;

public interface IGenericRepository<TEntity> 
    where TEntity : class
{
    Task AddAsync(TEntity entity);
    void Remove(TEntity entity);
    void Update(TEntity entity);
    Task<bool> AnyAsync();
    Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);
    Task<TEntity?> FindAsync(Guid id);
    Task<TEntity?> FirstOrDefaultAsync();
    Task<List<TEntity>> ToListAsync();
    Task SaveChangesAsync();
}
