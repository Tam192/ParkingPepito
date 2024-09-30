using Core.Interfaces.DbContext;
using System.Linq.Expressions;

namespace Core.Interfaces.Repository
{
    public interface IViewsRepository<TView> where TView : class, IView
    {
        Task<List<TView>> GetAllAsync(Expression<Func<TView, bool>>? filter = null, Func<IQueryable<TView>, IOrderedQueryable<TView>>? orderBy = null);
        Task<TView?> GetAsync(Expression<Func<TView, bool>>? filter, Func<IQueryable<TView>, IOrderedQueryable<TView>>? orderBy);
    }
}
