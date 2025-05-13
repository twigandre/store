using Store.App.Crosscutting.Commom.ViewModel.Core.Usuario.Listar;
using Store.App.Crosscutting.Commom.ViewModel.Pagination;

namespace Store.App.Core.Application.Usuario.Listar
{
    public class ListarUsuarioResponse
    {
        public PagedItems<ListarUsuarioResult> data { get; set; }
    }
}
