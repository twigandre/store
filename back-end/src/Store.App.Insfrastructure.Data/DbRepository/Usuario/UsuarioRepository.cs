using Microsoft.EntityFrameworkCore;
using Store.App.Crosscutting.Commom.Security.JwtManager;
using Store.App.Crosscutting.Commom.ViewModel.Core.Usuario.Listar;
using Store.App.Crosscutting.Commom.ViewModel.Pagination;
using Store.App.Infrastructure.Database.DbEntities;
using System.Threading;

namespace Store.App.Infrastructure.Database.DbRepository.Usuario
{
    public class UsuarioRepository : GenericRepository<UsuarioEntity>, IUsuarioRepository
    {
        IJwtManager _jwtManager;
        public UsuarioRepository(IJwtManager jwtManager, StoreContext db_dbContext) : base(db_dbContext)
        {
            _jwtManager = jwtManager;
        }

        public async Task<PagedItems<UsuarioEntity>> ListarUsuariosPaginado(ListarUsuarioRequest requestParams, CancellationToken cancellation)
        {
            var query = Query;

            query = query.Include(x => x.Endereco);

            query = query.Include(x => x.Filial);

            if (!string.IsNullOrEmpty(requestParams?.NomeUsuario))
            {
                query = query.Where(x => x.PrimeiroNome.ToUpper().Contains(requestParams.NomeUsuario) ||
                                         x.SobreNome.ToUpper().Contains(requestParams.NomeUsuario));
            }

            PagedItems<UsuarioEntity> resultadoPaginacao = await Pagination<UsuarioEntity>(requestParams, query, cancellation);

            return resultadoPaginacao;
        }

        public async Task<UsuarioEntity> UsuarioLogado(CancellationToken cancellationToken)
        {
            string emailUsuarioLogado = _jwtManager.ObterUsuarioLogado().EmailUsuarioLogado.ToLower();

            UsuarioEntity? usuarioLogadoFromDb = await Selecionar(x => x.Email.ToLower().Equals(emailUsuarioLogado), cancellationToken);

            return usuarioLogadoFromDb;
        }
    }
}
