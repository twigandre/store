using Store.App.Core.Domain.Entitites;
using Store.App.Core.Domain.Repositories;
using Store.App.Infrastructure.Context;

namespace Store.App.Infrastrucutre.Repositories.ItensVenda
{
    public class ItensVendaRepository : GenericRepository<VendaItensEntity>, IItensVendaRepository
    {
        public ItensVendaRepository(DafaultContext db_dbContext) : base(db_dbContext)
        {
        }
    }
}
