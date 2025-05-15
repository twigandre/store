using Store.App.Core.Domain.Entitites;

namespace Store.App.Core.Domain.Repositories
{
    public interface IUsuarioEnderecoRepository : IGenericRepository<UsuarioEnderecoEntity>
    {
        Task<int> SalvarEnderecoUsuario(UsuarioEnderecoEntity obj, CancellationToken cancelation);
    }
}
