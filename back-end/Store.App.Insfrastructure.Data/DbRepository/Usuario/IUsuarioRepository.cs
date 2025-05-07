using Store.App.Crosscutting.Commom.ViewModel.Core.Application.Usuario.Listar;
using Store.App.Crosscutting.Commom.ViewModel.Pagination;
using Store.App.Infrastructure.Database.DbEntities;

namespace Store.App.Infrastructure.Database.DbRepository.Usuario
{
    public interface IUsuarioRepository : IGenericRepository<UsuarioEntity>
    {
      Task<PagedItems<UsuarioEntity>> ListarUsuariosPaginado(ListarUsuarioRequest requestParams, CancellationToken cancellation);
    }
}
