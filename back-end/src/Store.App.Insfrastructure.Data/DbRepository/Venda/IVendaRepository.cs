using Store.App.Crosscutting.Commom.ViewModel;
using Store.App.Infrastructure.Database.DbEntities;

namespace Store.App.Infrastructure.Database.DbRepository.Venda
{
    public interface IVendaRepository : IGenericRepository<VendaEntity>
    {
        Task<RequestResponseVM> FinalizarCompra(CarroEntity carro, CancellationToken cancellationToken);
    }
}
