using Store.App.Crosscutting.Commom.ViewModel.Core.Produto.Listar;
using Store.App.Crosscutting.Commom.ViewModel.Pagination;
using Store.App.Infrastructure.Database.DbEntities;

namespace Store.App.Infrastructure.Database.DbRepository.Produto
{
    public interface IProdutoRepository : IGenericRepository<ProdutoEntity>
    {
        Task<PagedItems<ProdutoEntity>> ListarUsuariosPaginado(ListarProdutoRequest requestParams, CancellationToken cancellation);
    }
}
