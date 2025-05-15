using Store.App.Infrastructure.Context;
using Store.App.Crosscutting.Commom.Utils;
using Store.App.Crosscutting.Commom.ViewModel.Pagination;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Store.App.Core.Domain.Repositories;

namespace Store.App.Infrastrucutre.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        public DafaultContext Context { get; private set; }

        protected DbSet<T> Set => Context.Set<T>();

        public GenericRepository(DafaultContext db_dbContext)
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
        
        private void Adicionar(T entity)
        {
            var entry = Context.Entry(entity);
            if (entry.State == EntityState.Detached)            
                Set.Add(entity);            
        }

        private void Update(T entity)
        {
            var entry = Context.Entry(entity);
            if (entry.State == EntityState.Detached)
                Set.Attach(entity);

            entry.State = EntityState.Modified;
        }

        public async Task<T> Selecionar([Required] Expression<Func<T, bool>> predicate,
                                        CancellationToken cancellationToken = default,
                                        string includeProperties = "")
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
                                          CancellationToken cancellationToken = default,
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

            return await query
                   .AsNoTracking()
                   .ToListAsync(cancellationToken);            
        }

        public async Task<PagedItems<T>> Pagination<TResult>(PagedOptions pagedFilter, 
                                                             IQueryable<T> query, 
                                                             CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(pagedFilter.Sort))
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
                bool isReverse = pagedFilter.Reverse is null ? false : (bool)pagedFilter.Reverse;

                query = LinqExtension.OrderBy(query, pagedFilter.Sort, isReverse);
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

        public void Apagar(T entity)
        {
            var entry = Context.Entry(entity);
            if (entry.State == EntityState.Detached)
                Set.Attach(entity);

            Set.Remove(entity);
        }

        public void RemoveRange<T>(IEnumerable<T> entities) where T : class =>        
            Context.Set<T>().RemoveRange(entities);

        public async Task<int> CountAsync<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            return await Context.Set<T>().CountAsync(predicate);
        }

        public async Task SaveChangesAsync (CancellationToken token) => await Context.SaveChangesAsync(token);
    }
}
