using Store.App.Infrastructure.Database.DbEntities;

namespace Store.App.Infrastructure.Database.DbRepository.Carro
{
    public interface ICarroRepository : IGenericRepository<CarroEntity>
    {
        Task<CarroEntity> SelecionarCarroPorIdProduto(int idProduto, CancellationToken token);
    }
}
