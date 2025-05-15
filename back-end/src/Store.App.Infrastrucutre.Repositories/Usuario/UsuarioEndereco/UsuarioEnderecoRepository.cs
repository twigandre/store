using Store.App.Core.Domain.Entitites;
using Store.App.Core.Domain.Repositories;
using Store.App.Infrastructure.Context;

namespace Store.App.Infrastrucutre.Repositories.UsuarioEndereco
{
    public class UsuarioEnderecoRepository : GenericRepository<UsuarioEnderecoEntity>, IUsuarioEnderecoRepository
    {
        public UsuarioEnderecoRepository(DafaultContext db_dbContext) : base(db_dbContext)
        {
        }

        public async Task<int> SalvarEnderecoUsuario(UsuarioEnderecoEntity obj, CancellationToken cancelation)
        {
            UsuarioEnderecoEntity entity = new UsuarioEnderecoEntity
            {
                Cidade = obj.Cidade,
                ZipCode = obj.ZipCode,
                NumeroImovel = obj.NumeroImovel,
                Latitude = obj.Latitude,
                Logradouro = obj.Logradouro,
                Longitude = obj.Longitude,
                Id = obj.Id,
                UsuarioId = obj.UsuarioId
            };

            Salvar(entity);
            await SaveChangesAsync(cancelation);

            return entity.Id;
        }
    }
}
