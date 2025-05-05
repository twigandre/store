using Store.App.Infrastructure.Database.Entities;

namespace Store.App.Infrastructure.Database.DbRepository.Produto
{
    public class ProdutoRepository : GenericRepository<ProdutoEntity>, IProdutoRepository
    {
        public ProdutoRepository(StoreContext db_dbContext) : base(db_dbContext)
        {
        }
    }
}
