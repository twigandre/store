using Store.App.Infrastructure.Database.Entities;

namespace Store.App.Infrastructure.Database.DbRepository.Usuario
{
    public class UsuarioRepository : GenericRepository<UsuarioEntity>, IUsuarioRepository
    {
        public UsuarioRepository(StoreContext db_dbContext) : base(db_dbContext)
        {
        }
    }
}
