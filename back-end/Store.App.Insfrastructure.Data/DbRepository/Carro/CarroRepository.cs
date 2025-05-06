using Store.App.Infrastructure.Database;
using Store.App.Infrastructure.Database.Entities;

namespace Store.App.Infrastructure.Database.DbRepository.Carro
{
    public class CarroRepository : GenericRepository<CarroEntity>, ICarroRepository
    {
        public CarroRepository(StoreContext db_dbContext) : base(db_dbContext)
        {
        }
    }
}
