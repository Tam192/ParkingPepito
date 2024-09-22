using Core.Interfaces.DbContext;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace Core.Interfaces.Repository
{
    public interface IEntitiesRepository<TEntity> where TEntity : class, IEntity
    {
        Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, string? includeProperties = "");
        Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>>? filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy, string? includeProperties);
        Task<EntityEntry<TEntity>> CreateAsync(TEntity entity);
        Task<EntityEntry<TEntity>> RemoveAsync(TEntity entity);
        Task<EntityEntry<TEntity>> UpdateAsync(TEntity entity);
        Task<int> SaveAsync();
    }
}
