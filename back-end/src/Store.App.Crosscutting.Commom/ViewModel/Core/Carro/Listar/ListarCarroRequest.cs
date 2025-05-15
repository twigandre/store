using Store.App.Crosscutting.Commom.ViewModel.Pagination;

namespace Store.App.Crosscutting.Commom.ViewModel.Core.Carro.Listar
{
    public class ListarCarroRequest : PagedOptions
    {

        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public int? IdUsuario { get; set; }
        public int? IdProduto { get; set; }
    }
}
