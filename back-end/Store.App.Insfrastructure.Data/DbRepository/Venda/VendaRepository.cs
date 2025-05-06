using Store.App.Infrastructure.Database.Entities;

namespace Store.App.Infrastructure.Database.DbRepository.Venda
{
    public class VendaRepository : GenericRepository<VendaEntity>, IVendaRepository
    {
        public VendaRepository(StoreContext db_dbContext) : base(db_dbContext)
        {
        }
    }
}
