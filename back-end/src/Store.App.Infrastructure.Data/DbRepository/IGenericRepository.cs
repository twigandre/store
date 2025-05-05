using Store.App.Crosscutting.Commom.ViewModel.Pagination;
using System.Linq.Expressions;

namespace Store.App.Infrastructure.Database.DbRepository
{
    public interface IGenericRepository<T> where T : class
    {
        void Salvar(T entity);
        void Update(T entity);
        void Apagar(T entity);
        T Find(params object[] keys);
        public List<T> Listar(Expression<Func<T, bool>> filter = null,
               Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
               string includeProperties = "");
        public T Selecionar(Expression<Func<T, bool>> predicate, string includeProperties = "");
        Task<PagedItems<T>> PaginationQueryRepository<TResult>(PagedOptions pagedFilter, IQueryable<T> query);
        public PagedItems<T> ListarPaginado(Expression<Func<T, bool>> predicate, PagedOptions pagedFilter);
        IEnumerable<T> FindAll();
        IQueryable<T> Query { get; }
        StoreContext Context { get; }
        bool Existe(Expression<Func<T, bool>> predicate);
        void DetachEntries();
        void DetachEspecifyEntity(T entity);
    }
}
