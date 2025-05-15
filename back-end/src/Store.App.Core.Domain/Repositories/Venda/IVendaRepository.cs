using Store.App.Core.Domain.Entitites;
using Store.App.Crosscutting.Commom.ViewModel;

namespace Store.App.Core.Domain.Repositories.Venda
{
    public interface IVendaRepository : IGenericRepository<VendaEntity>
    {
        Task<RequestResponseVM> FinalizarCompra(CarroEntity carro, CancellationToken cancellationToken);
    }
}
