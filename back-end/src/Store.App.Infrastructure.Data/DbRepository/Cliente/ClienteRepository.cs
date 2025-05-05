using Store.App.Infrastructure.Database.Entities;

namespace Store.App.Infrastructure.Database.DbRepository.Cliente
{
    public class ClienteRepository : GenericRepository<ClienteEntity>, IClienteRepository
    {
        public ClienteRepository(StoreContext db_dbContext) : base(db_dbContext)
        {
        }
    }
}
