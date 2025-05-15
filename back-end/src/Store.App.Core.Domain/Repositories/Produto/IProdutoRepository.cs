using Store.App.Core.Domain.Entitites;
using Store.App.Crosscutting.Commom.ViewModel.Core.Produto.Listar;
using Store.App.Crosscutting.Commom.ViewModel.Pagination;

namespace Store.App.Core.Domain.Repositories
{
    public interface IProdutoRepository : IGenericRepository<ProdutoEntity>
    {
        Task<PagedItems<ProdutoEntity>> ListarUsuariosPaginado(ListarProdutoRequest requestParams, CancellationToken cancellation);
    }
}
