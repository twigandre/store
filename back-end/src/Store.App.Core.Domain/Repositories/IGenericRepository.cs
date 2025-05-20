using Store.App.Crosscutting.Commom.ViewModel.Pagination;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace Store.App.Core.Domain.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        IQueryable<T> Query { get; }
        void Salvar(T entity);
        void Apagar(T entity);
        Task<IEnumerable<T>> FindAll(CancellationToken cancellationToken);
        Task<List<T>> Listar(Expression<Func<T, bool>> filter,
                             CancellationToken cancellationToken,
                             string includeProperties);
        Task<T> Selecionar(Expression<Func<T, bool>> predicate,
                           CancellationToken cancellationToken,
                           string includeProperties);
        Task<PagedItems<T>> Pagination<TResult>(PagedOptions pagedFilter, 
                                                IQueryable<T> query,
                                                CancellationToken cancellationToken);        
        Task<bool> Existe(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken);
        void DetachEntries();
        void DetachEspecifyEntity(T entity);
        void RemoveRange<T>(IEnumerable<T> entities) where T : class;
        Task<int> CountAsync<T>(Expression<Func<T, bool>> predicate) where T : class;
        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
