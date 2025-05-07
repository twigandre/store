using Store.App.Infrastructure.Database.DbEntities;
using Store.App.Infrastructure.Database.DbRepository;

namespace Store.App.Insfrastructure.Database.DbRepository.Usuario.UsuarioEndereco
{
    public interface IUsuarioEnderecoRepository : IGenericRepository<UsuarioEnderecoEntity>
    {
        Task<int> SalvarEnderecoUsuario(UsuarioEnderecoEntity obj, CancellationToken cancelation);
    }
}
