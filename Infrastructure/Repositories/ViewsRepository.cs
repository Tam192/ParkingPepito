using Core.Interfaces.Repository;
using Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Repositories
{
    public class ViewsRepository<TEntity> : IViewsRepository<TEntity> where TEntity : class, IDisposable
    {
        internal ParkingPepitoDbContext _context;
        internal DbSet<TEntity> dbSet;
        public static char[] Separator => [','];

        public ViewsRepository(ParkingPepitoDbContext context)
        {
            _context = context;
            dbSet = _context.Set<TEntity>();
        }

        async Task<List<TEntity>> IViewsRepository<TEntity>.GetAllAsync(Expression<Func<TEntity, bool>>? filter,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy)
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return await (orderBy != null ? orderBy(query).ToListAsync() : query.ToListAsync());
        }

        async Task<TEntity?> IViewsRepository<TEntity>.GetAsync(Expression<Func<TEntity, bool>>? filter,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy)
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return await (orderBy != null ? orderBy(query).FirstOrDefaultAsync() : query.FirstOrDefaultAsync());
        }
    }
}
