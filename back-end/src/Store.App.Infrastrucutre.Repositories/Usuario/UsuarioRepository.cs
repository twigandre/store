using Microsoft.EntityFrameworkCore;
using Store.App.Core.Domain.Entitites;
using Store.App.Core.Domain.Repositories;
using Store.App.Crosscutting.Commom.Security.JwtManager;
using Store.App.Crosscutting.Commom.ViewModel.Core.Usuario.Listar;
using Store.App.Crosscutting.Commom.ViewModel.Pagination;
using Store.App.Infrastructure.Context;

namespace Store.App.Infrastrucutre.Repositories.Usuario
{
    public class UsuarioRepository : GenericRepository<UsuarioEntity>, IUsuarioRepository
    {
        IJwtManager _jwtManager;
        public UsuarioRepository(IJwtManager jwtManager, DafaultContext db_dbContext) : base(db_dbContext)
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

            UsuarioEntity? usuarioLogadoFromDb = await Selecionar(x => x.Email.ToLower().Equals(emailUsuarioLogado), cancellationToken, string.Empty);

            return usuarioLogadoFromDb;
        }
    }
}
