using Store.App.Infrastructure.Database;
using Store.App.Infrastructure.Database.DbEntities;
using Store.App.Infrastructure.Database.DbRepository;

namespace Store.App.Insfrastructure.Database.DbRepository.Usuario.UsuarioEndereco
{
    public class UsuarioEnderecoRepository : GenericRepository<UsuarioEnderecoEntity>, IUsuarioEnderecoRepository
    {
        public UsuarioEnderecoRepository(StoreContext db_dbContext) : base(db_dbContext)
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

            Salvar(entity, cancelation);
            await Context.SaveChangesAsync(cancelation);

            return entity.Id;
        }
    }
}
