using Store.App.Crosscutting.Commom.ViewModel.Pagination;
using System.ComponentModel.DataAnnotations;

namespace Store.App.Crosscutting.Commom.ViewModel.Core.Usuario.Listar
{
    public class ListarUsuarioRequest : PagedOptions
    {
        [StringLength(20)]
        public string? NomeUsuario { get; set; } = string.Empty;
    }
}
