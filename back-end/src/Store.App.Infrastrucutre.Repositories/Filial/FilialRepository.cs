using Store.App.Core.Domain.Entitites;
using Store.App.Core.Domain.Repositories;
using Store.App.Infrastructure.Context;

namespace Store.App.Infrastrucutre.Repositories.Filial
{
    public class FilialRepository : GenericRepository<FilialEntity>, IFilialRepository
    {
        public FilialRepository(DafaultContext db_dbContext) : base(db_dbContext)
        {
        }
    }
}
