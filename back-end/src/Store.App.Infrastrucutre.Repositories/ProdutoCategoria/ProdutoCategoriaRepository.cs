using Store.App.Core.Domain.Entitites;
using Store.App.Core.Domain.Repositories;
using Store.App.Infrastructure.Context;

namespace Store.App.Infrastrucutre.Repositories.ProdutoCategoria
{
    public class ProdutoCategoriaRepository : GenericRepository<ProdutoCategoriaEntity>, IProdutoCategoriaRepository
    {
        public ProdutoCategoriaRepository(DafaultContext db_dbContext) : base(db_dbContext)
        {
        }
    }
}
