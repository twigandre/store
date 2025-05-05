using Microsoft.EntityFrameworkCore;
using Store.App.Crosscutting.Commom.Utils;
using Store.App.Crosscutting.Commom.ViewModel.Pagination;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace Store.App.Infrastructure.Database.DbRepository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        public StoreContext Context { get; private set; }

        protected DbSet<T> Set => Context.Set<T>();

        public GenericRepository(StoreContext db_dbContext)
        {
            Context = db_dbContext;
        }

        public IQueryable<T> Query => Set;

        public T Find(params object[] keys)
        {
            return Set.Find(keys);
        }

        public IEnumerable<T> FindAll()
        {
            var teste = Set.AsNoTracking().ToList();
            return teste;
        }

        public void Adicionar(T entity)
        {
            var entry = Context.Entry(entity);
            if (entry.State == EntityState.Detached)
                Set.Add(entity);
        }

        public void Salvar(T entity)
        {
            var props = typeof(T)
                .GetProperties()
                .Where(prop =>
                    Attribute.IsDefined(prop,
                        typeof(KeyAttribute)));

            var codeValue = props.First().GetValue(entity).GetType().Name == "Int64" ? (long)props.First().GetValue(entity) : (int)props.First().GetValue(entity);

            if (codeValue == 0)
            {
                Adicionar(entity);
            }
            else
            {
                Update(entity);
            }
        }

        public void Apagar(T entity)
        {
            var entry = Context.Entry(entity);
            if (entry.State == EntityState.Detached)
                Set.Attach(entity);
            Set.Remove(entity);
        }

        public void Update(T entity)
        {
            var entry = Context.Entry(entity);
            if (entry.State == EntityState.Detached)
                Set.Attach(entity);
            entry.State = EntityState.Modified;
        }

        public T Selecionar([Required] Expression<Func<T, bool>> predicate, string includeProperties = "")
        {
            IQueryable<T> query = Context.Set<T>();

            query = query.Where(predicate);

            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            return query
                   .AsNoTracking()
                   .FirstOrDefault();
        }

        public List<T> Listar(Expression<Func<T, bool>> filter = null,
               Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<T> query = Context.Set<T>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query)
                    .AsNoTracking()
                    .ToList();
            }
            else
            {
                return query
                    .AsNoTracking()
                    .ToList();
            }
        }

        public async Task<PagedItems<T>> PaginationQueryRepository<TResult>(PagedOptions pagedFilter, IQueryable<T> query)
        {
            if (string.IsNullOrEmpty(pagedFilter.Sort) && pagedFilter.SortManny == null)
            {
                var props = typeof(T)
                    .GetProperties()
                    .Where(prop =>
                        Attribute.IsDefined(prop,
                            typeof(KeyAttribute)));

                pagedFilter.Sort = props.First().Name;
            }

            PagedItems<T> paged = new PagedItems<T>();

            paged.Total = query.Count();

            if (!string.IsNullOrEmpty(pagedFilter.Sort))
            {
                query = LinqExtension.OrderBy(query, pagedFilter.Sort, pagedFilter.Reverse == null ? false : (bool)pagedFilter.Reverse);
            }
            else
            {
                if (pagedFilter.SortManny != null)
                {
                    var list = pagedFilter.SortManny.ToList();
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (i == 0)
                        {
                            query = LinqExtension.OrderBy(query, list[i].Sort, list[i].Reverse == null ? false : (bool)list[i].Reverse);
                        }
                        else
                        {
                            query = LinqExtension.ThenBy(query, list[i].Sort, list[i].Reverse == null ? false : (bool)list[i].Reverse);
                        }
                    }
                }
            }

            var skip = pagedFilter.Page.Value * pagedFilter.Size.Value - pagedFilter.Size.Value;
            query = query.Skip(skip);
            query = query.Take(pagedFilter.Size.Value);

            var resultadoBusca = await query.AsNoTracking().ToListAsync();

            paged.Items = resultadoBusca;

            return paged;
        }

        public PagedItems<T> ListarPaginado(Expression<Func<T, bool>> predicate, PagedOptions pagedFilter)
        {

            if (string.IsNullOrEmpty(pagedFilter.Sort) && pagedFilter.SortManny == null)
            {
                var props = typeof(T)
                    .GetProperties()
                    .Where(prop =>
                        Attribute.IsDefined(prop,
                            typeof(KeyAttribute)));

                pagedFilter.Sort = props.First().Name;
            }

            PagedItems<T> paged = new PagedItems<T>();

            IQueryable<T> query = Context.Set<T>();

            query = Context
                    .Set<T>()
                    .AsNoTracking()
                    .Where(predicate)
                    .AsQueryable();

            paged.Total = query.Count();

            if (!string.IsNullOrEmpty(pagedFilter.Sort))
            {
                query = LinqExtension.OrderBy(query, pagedFilter.Sort, pagedFilter.Reverse == null ? false : (bool)pagedFilter.Reverse);
            }
            else
            {
                if (pagedFilter.SortManny != null)
                {
                    var list = pagedFilter.SortManny.ToList();
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (i == 0)
                        {
                            query = LinqExtension.OrderBy(query, list[i].Sort, list[i].Reverse == null ? false : (bool)list[i].Reverse);
                        }
                        else
                        {
                            query = LinqExtension.ThenBy(query, list[i].Sort, list[i].Reverse == null ? false : (bool)list[i].Reverse);
                        }
                    }
                }
            }

            var skip = pagedFilter.Page.Value * pagedFilter.Size.Value - pagedFilter.Size.Value;
            query = query.Skip(skip);
            query = query.Take(pagedFilter.Size.Value);

            paged.Items = query.ToList();
            return paged;
        }

        public bool Existe(Expression<Func<T, bool>> predicate)
        {
            return Context
                .Set<T>()
                .AsNoTracking()
                .Any(predicate);
        }

        public void DetachEntries()
        {
            foreach (var entry in Context.ChangeTracker.Entries())
            {
                entry.State = EntityState.Detached;
            }
        }

        public void DetachEspecifyEntity(T entity)
        {
            Context.Entry(entity).State = EntityState.Detached;
        }
    }
}
