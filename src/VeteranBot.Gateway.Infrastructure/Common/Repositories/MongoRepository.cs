using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using VeteranBot.Gateway.Domain.Common.Errors;
using VeteranBot.Gateway.Domain.Common.Exceptions;
using VeteranBot.Gateway.Domain.Common.Repositories;

namespace VeteranBot.Gateway.Infrastructure.Common.Repositories;

public class MongoRepository<TEntity, TDbContext> : IRepository<TEntity> where TEntity : class
{
    private readonly IMongoCollection<TEntity> _mongoCollection;

    public MongoRepository(IMongoCollection<TEntity> mongoCollection)
    {
        _mongoCollection = mongoCollection;
    }

    public async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken)
    {
        await _mongoCollection.InsertOneAsync(entity, cancellationToken: cancellationToken);

        return entity;
    }

    public async Task<TEntity> UpdateAsync(
        Expression<Func<TEntity, bool>> where, 
        TEntity entity, 
        CancellationToken cancellationToken
        )
    
    {
        await _mongoCollection.ReplaceOneAsync(
            where,
            entity,
            cancellationToken: cancellationToken
            );

        return entity;
    }

    public async Task<TEntity> FirstAsync(ICollection<Expression<Func<TEntity, bool>>>? wheres = null, CancellationToken cancellationToken = default)
    {
        var entity = await FirstOrDefaultAsync(wheres, cancellationToken);

        if (entity is null)
        {
            throw new BusinessException
            {
                ErrorCode = ErrorCode.NotFound,
                ErrorKind = ErrorKind.NotFound
            };
        }

        return entity;
    }

    public async Task<TEntity?> FirstOrDefaultAsync(ICollection<Expression<Func<TEntity, bool>>>? wheres = null, CancellationToken cancellationToken = default)
    {
        var queryable = _mongoCollection.AsQueryable();
            
        if (wheres is not null)
        {
            queryable = wheres.Aggregate(queryable, (accumulator, expression) => accumulator.Where(expression));
        }
        
        var entity = await IAsyncCursorSourceExtensions.FirstOrDefaultAsync(queryable, cancellationToken);

        return entity;
    }

    public async Task<bool> AnyAsync(ICollection<Expression<Func<TEntity, bool>>>? wheres = null, CancellationToken cancellationToken = default)
    {
        var queryable = _mongoCollection.AsQueryable();
            
        if (wheres is not null)
        {
            queryable = wheres.Aggregate(queryable, (accumulator, expression) => accumulator.Where(expression));
        }
        
        var any = await IAsyncCursorSourceExtensions.AnyAsync(queryable, cancellationToken);

        return any;
    }
}