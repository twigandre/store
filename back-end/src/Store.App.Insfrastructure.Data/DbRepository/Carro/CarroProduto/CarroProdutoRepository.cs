using Store.App.Infrastructure.Database.DbEntities;
using Store.App.Infrastructure.Database.DbRepository;
using Store.App.Infrastructure.Database;

namespace Store.App.Insfrastructure.Database.DbRepository.Carro.CarroProduto
{
    public class CarroProdutoRepository : GenericRepository<CarroProdutoEntity>, ICarroProdutoRepository
    {
        public CarroProdutoRepository(StoreContext db_dbContext) : base(db_dbContext)
        {
        }
    }
}
