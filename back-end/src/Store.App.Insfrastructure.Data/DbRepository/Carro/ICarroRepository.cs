using Store.App.Crosscutting.Commom.ViewModel.Core.Carro.Listar;
using Store.App.Crosscutting.Commom.ViewModel.Pagination;
using Store.App.Infrastructure.Database.DbEntities;

namespace Store.App.Infrastructure.Database.DbRepository.Carro
{
    public interface ICarroRepository : IGenericRepository<CarroEntity>
    {
        Task<CarroEntity?> RecuperarCarroAtivoDoUsuarioLogado(CancellationToken cancellationToken);
        Task<CarroEntity> CriarCarroParaUsuario(int idUsuario, CancellationToken cancelation);
        Task<CarroEntity> SelecionarCarroPorIdProduto(int idProduto, CancellationToken token);
        Task<PagedItems<CarroEntity>> ListarPaginado(ListarCarroRequest request, CancellationToken cancellation);
    }
}
