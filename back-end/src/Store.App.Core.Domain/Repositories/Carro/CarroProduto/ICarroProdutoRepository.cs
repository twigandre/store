using Store.App.Core.Domain.Entitites;
using Store.App.Crosscutting.Commom.ViewModel;
using Store.App.Crosscutting.Commom.ViewModel.Core.Carro;

namespace Store.App.Core.Domain.Repositories
{
    public interface ICarroProdutoRepository : IGenericRepository<CarroProdutoEntity>
    {
        Task<RequestResponseVM> IncluirProdutoNoCarro(ManterCarroProdutoVM obj, CancellationToken cancellationToken);
        Task<RequestResponseVM> RemoverProdutoDoCarro(ManterCarroProdutoVM obj, CancellationToken cancellationToken);
        Task<List<int>> RecuperarIdsDeCarrosQueContemDeterminadoProduto(int idProduto, CancellationToken cancellationToken);
    }
}
