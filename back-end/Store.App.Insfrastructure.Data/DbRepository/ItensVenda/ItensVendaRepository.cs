using Store.App.Infrastructure.Database.Entities;

namespace Store.App.Infrastructure.Database.DbRepository.ItensVenda
{
    public class ItensVendaRepository : GenericRepository<VendaItensEntity>, IItensVendaRepository
    {
        public ItensVendaRepository(StoreContext db_dbContext) : base(db_dbContext)
        {
        }
    }
}
