using Microsoft.EntityFrameworkCore;
using Store.App.Crosscutting.Commom.ViewModel.Core.Application.Usuario.Listar;
using Store.App.Crosscutting.Commom.ViewModel.Pagination;
using Store.App.Infrastructure.Database.DbEntities;

namespace Store.App.Infrastructure.Database.DbRepository.Usuario
{
    public class UsuarioRepository : GenericRepository<UsuarioEntity>, IUsuarioRepository
    {
        public UsuarioRepository(StoreContext db_dbContext) : base(db_dbContext)
        {
        }

        public async Task<PagedItems<UsuarioEntity>> ListarUsuariosPaginado(ListarUsuarioRequest requestParams, CancellationToken cancellation)
        {
            var query = Query;
                                    
            query = query.Include(x => x.Endereco);
            
            query = query.Include(x => x.Filial);

            if (!string.IsNullOrEmpty(requestParams?.NomeUsuario))
            {
                query = query.Where(x => x.PrimeiroNome.ToUpper().Contains(requestParams.NomeUsuario) ||
                                         x.SobreNome.ToUpper().Contains(requestParams.NomeUsuario) );
            }                

            PagedItems<UsuarioEntity> resultadoPaginacao = await Pagination<UsuarioEntity>(requestParams, query, cancellation);

            return resultadoPaginacao;
        }
    }
}
