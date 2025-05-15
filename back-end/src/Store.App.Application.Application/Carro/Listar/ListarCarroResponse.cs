using Store.App.Crosscutting.Commom.ViewModel;
using Store.App.Crosscutting.Commom.ViewModel.Core.Carro.Listar;
using Store.App.Crosscutting.Commom.ViewModel.Pagination;

namespace Store.App.Core.Application.Carro.Listar
{
    public class ListarCarroResponse : RequestResponseVM
    {
        public PagedItems<ListarCarroResult>? data { get; set; }
    }
}
