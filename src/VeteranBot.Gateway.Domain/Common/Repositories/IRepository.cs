using System.Linq.Expressions;

namespace VeteranBot.Gateway.Domain.Common.Repositories;

public interface IRepository<TEntity> 
{
    Task<TEntity> AddAsync(
        TEntity entity, 
        CancellationToken cancellationToken
        );
    Task<TEntity> UpdateAsync(
        Expression<Func<TEntity, bool>> where, 
        TEntity entity, 
        CancellationToken cancellationToken
        );
    
    Task<TEntity> FirstAsync(
        ICollection<Expression<Func<TEntity, bool>>>? wheres = null,
        CancellationToken cancellationToken = default
        );
    Task<TEntity?> FirstOrDefaultAsync(
        ICollection<Expression<Func<TEntity, bool>>>? wheres = null,
        CancellationToken cancellationToken = default
        );
    Task<bool> AnyAsync(
        ICollection<Expression<Func<TEntity, bool>>>? wheres = null, 
        CancellationToken cancellationToken = default
        );
}