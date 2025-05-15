using Store.App.Crosscutting.Commom.ViewModel;
using Store.App.Crosscutting.Commom.ViewModel.Core.Carro;
using Store.App.Infrastructure.Database.DbEntities;
using Store.App.Infrastructure.Database.DbRepository;

namespace Store.App.Insfrastructure.Database.DbRepository.Carro.CarroProduto
{
    public interface ICarroProdutoRepository : IGenericRepository<CarroProdutoEntity>
    {
        Task<RequestResponseVM> IncluirProdutoNoCarro(ManterCarroProdutoVM obj, CancellationToken cancellationToken);
        Task<RequestResponseVM> RemoverProdutoDoCarro(ManterCarroProdutoVM obj, CancellationToken cancellationToken);
        Task<List<int>> RecuperarIdsDeCarrosQueContemDeterminadoProduto(int idProduto, CancellationToken cancellationToken);
    }
}
