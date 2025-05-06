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

        public async Task<IEnumerable<T>> FindAll(CancellationToken cancellationToken) =>
            await Set.AsNoTracking()    
                     .ToListAsync(cancellationToken);
         
        public async Task Salvar(T entity, CancellationToken cancellationToken)
        {
            var props = typeof(T)
                .GetProperties()
                .Where(prop =>
                    Attribute.IsDefined(prop,
                        typeof(KeyAttribute)));

            var codeValue = props.First().GetValue(entity).GetType().Name == "Int64" ? (long)props.First().GetValue(entity) : (int)props.First().GetValue(entity);

            if (codeValue == 0)
            {
                await Adicionar(entity);
            }
            else
            {
                await Update(entity);
            }
        }
        
        private async Task Adicionar(T entity)
        {
            var entry = Context.Entry(entity);
            if (entry.State == EntityState.Detached)            
                await Set.AddAsync(entity);            
        }

        private Task Update(T entity)
        {
            var entry = Context.Entry(entity);
            if (entry.State == EntityState.Detached)
                Set.Attach(entity);

            entry.State = EntityState.Modified;

            return Task.CompletedTask;
        }

        public async Task<T> Selecionar([Required] Expression<Func<T, bool>> predicate, 
                                        string includeProperties = "", 
                                        CancellationToken cancellationToken = default)
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

            return await query
                         .AsNoTracking()
                         .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<List<T>> Listar(Expression<Func<T, bool>> filter = null, 
                                          string includeProperties = "", 
                                          CancellationToken cancellationToken = default)
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

            return await query
                   .AsNoTracking()
                   .ToListAsync(cancellationToken);            
        }

        public async Task<PagedItems<T>> Pagination<TResult>(PagedOptions pagedFilter, 
                                                             IQueryable<T> query, 
                                                             CancellationToken cancellationToken = default)
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

            var resultadoBusca = await query.AsNoTracking().ToListAsync(cancellationToken);

            paged.Items = resultadoBusca;

            return paged;
        }

        public async Task<bool> Existe(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default) => 
            await Context.Set<T>()
                         .AsNoTracking()
                         .AnyAsync(predicate, cancellationToken);
        

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

        public Task Apagar(T entity, CancellationToken cancellationToken = default)
        {
            var entry = Context.Entry(entity);
            if (entry.State == EntityState.Detached)
                Set.Attach(entity);

            Set.Remove(entity);

            return Task.CompletedTask;
        }
    }
}
