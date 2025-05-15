using Microsoft.EntityFrameworkCore;
using Store.App.Core.Domain.Entitites;
using Store.App.Core.Domain.Repositories;
using Store.App.Crosscutting.Commom.ViewModel.Core.Produto.Listar;
using Store.App.Crosscutting.Commom.ViewModel.Pagination;
using Store.App.Infrastructure.Context;

namespace Store.App.Infrastrucutre.Repositories.Produto
{
    public class ProdutoRepository : GenericRepository<ProdutoEntity>, IProdutoRepository
    {
        public ProdutoRepository(DafaultContext db_dbContext) : base(db_dbContext)
        {
        }

        public async Task<PagedItems<ProdutoEntity>> ListarUsuariosPaginado(ListarProdutoRequest requestParams, CancellationToken cancellation)
        {
            IQueryable<ProdutoEntity> query = Query;

            query = query.Include(x => x.Carro);
            
            query = query.Include(x => x.Categoria);

            if (!string.IsNullOrEmpty(requestParams.Nome))
            {
                query = query.Where(x => x.Nome.ToLower().Contains(requestParams.Nome.ToLower()));
            }

            if (requestParams.PrecoMin is not null && requestParams.PrecoMax is not null)
            {
                query = query.Where(x => x.PrecoUnitario >= requestParams.PrecoMin && x.PrecoUnitario <= requestParams.PrecoMax);
            }
            else
            if (requestParams.PrecoMin is not null)
            {
                query = query.Where(x => x.PrecoUnitario >= requestParams.PrecoMin);
            }
            else 
            if (requestParams.PrecoMax is not null)
            {
                query = query.Where(x => x.PrecoUnitario <= requestParams.PrecoMax);
            }

            if (requestParams.CategoriaId is not null)
            {
                query = query.Where(x => x.CategoriaId == requestParams.CategoriaId);
            }

            if (!string.IsNullOrEmpty(requestParams.Nome))
            {
                query = query.Where(x => x.Nome.ToLower().Contains(requestParams.Nome.ToLower()));
            }

            PagedItems<ProdutoEntity> resultadoPaginacao = await Pagination<UsuarioEntity>(requestParams, query, cancellation);

            return resultadoPaginacao;
        }
    }
}
