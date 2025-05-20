using Store.App.Core.Domain.Entitites;
using Store.App.Core.Domain.Repositories;
using Store.App.Core.Domain.Repositories.Venda;
using Store.App.Crosscutting.Commom.ViewModel;
using Store.App.Infrastructure.Context;

namespace Store.App.Infrastrucutre.Repositories.Venda
{
    public class VendaRepository : GenericRepository<VendaEntity>, IVendaRepository
    {
        public VendaRepository(DafaultContext db_dbContext) : base(db_dbContext)
        {
        }
    } 
}
