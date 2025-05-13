using Store.App.Crosscutting.Commom.ViewModel.Pagination;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace Store.App.Infrastructure.Database.DbRepository
{
    public interface IGenericRepository<T> where T : class
    {
        IQueryable<T> Query { get; }
        StoreContext Context { get; }
        Task Salvar(T entity);
        Task Apagar(T entity);
        Task<IEnumerable<T>> FindAll(CancellationToken cancellationToken);
        Task<List<T>> Listar(Expression<Func<T, bool>> filter = null,
                             string includeProperties = "",
                             CancellationToken cancellationToken = default);
        Task<T> Selecionar(Expression<Func<T, bool>> predicate, 
                           string includeProperties = "", 
                           CancellationToken cancellationToken = default);
        Task<PagedItems<T>> Pagination<TResult>(PagedOptions pagedFilter, 
                                                IQueryable<T> query,
                                                CancellationToken cancellationToken = default);        
        Task<bool> Existe(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
        void DetachEntries();
        void DetachEspecifyEntity(T entity);
        void RemoveRange<T>(IEnumerable<T> entities) where T : class;
    }
}
