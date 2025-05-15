using Store.App.Core.Domain.Entitites;
using Store.App.Crosscutting.Commom.ViewModel.Core.Carro.Listar;
using Store.App.Crosscutting.Commom.ViewModel.Pagination;

namespace Store.App.Core.Domain.Repositories.Carro
{
    public interface ICarroRepository : IGenericRepository<CarroEntity>
    {
        Task<CarroEntity?> RecuperarCarroAtivoDoUsuarioLogado(CancellationToken cancellationToken);
        Task<CarroEntity> CriarCarroParaUsuario(int idUsuario, CancellationToken cancelation);
        Task<CarroEntity> SelecionarCarroPorIdProduto(int idProduto, CancellationToken token);
        Task<PagedItems<CarroEntity>> ListarPaginado(ListarCarroRequest request, CancellationToken cancellation);
    }
}
