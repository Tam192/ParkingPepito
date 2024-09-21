using Core.Interfaces.Repository;
using Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace Infrastructure.Repositories
{
    public class EntitiesRepository<TEntity> : IEntitiesRepository<TEntity> where TEntity : class, IDisposable
    {
        internal ParkingPepitoDbContext _context;
        internal DbSet<TEntity> dbSet;
        public static char[] Separator => [','];

        public EntitiesRepository(ParkingPepitoDbContext context)
        {
            _context = context;
            dbSet = _context.Set<TEntity>();
        }

        async Task<List<TEntity>> IEntitiesRepository<TEntity>.GetAllAsync(Expression<Func<TEntity, bool>>? filter,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy, string includeProperties)
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (string includeProperty in includeProperties.Split
                (Separator, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            return await (orderBy != null ? orderBy(query).ToListAsync() : query.ToListAsync());
        }

        async Task<TEntity?> IEntitiesRepository<TEntity>.GetAsync(Expression<Func<TEntity, bool>>? filter,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy, string includeProperties)
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (string includeProperty in includeProperties.Split
                (Separator, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            return await (orderBy != null ? orderBy(query).FirstOrDefaultAsync() : query.FirstOrDefaultAsync());
        }

        async Task<EntityEntry<TEntity>> IEntitiesRepository<TEntity>.CreateAsync(TEntity entity)
        {
            return await Task.FromResult(dbSet.Add(entity));
        }

        async Task<EntityEntry<TEntity>> IEntitiesRepository<TEntity>.RemoveAsync(TEntity entity)
        {
            return await Task.FromResult(dbSet.Remove(entity));
        }
        async Task<EntityEntry<TEntity>> IEntitiesRepository<TEntity>.UpdateAsync(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            return await Task.FromResult(dbSet.Attach(entity));
        }

        async Task<int> IEntitiesRepository<TEntity>.SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

    }
}
