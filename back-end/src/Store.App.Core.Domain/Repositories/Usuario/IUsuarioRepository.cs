using Store.App.Core.Domain.Entitites;
using Store.App.Crosscutting.Commom.ViewModel.Core.Usuario.Listar;
using Store.App.Crosscutting.Commom.ViewModel.Pagination;

namespace Store.App.Core.Domain.Repositories
{
    public interface IUsuarioRepository : IGenericRepository<UsuarioEntity>
    {
        Task<PagedItems<UsuarioEntity>> ListarUsuariosPaginado(ListarUsuarioRequest requestParams, CancellationToken cancellation);
        Task<UsuarioEntity> UsuarioLogado(CancellationToken cancellationToken);
    }
}
