using Store.App.Crosscutting.Commom.ViewModel.Pagination;
using System.ComponentModel.DataAnnotations;

namespace Store.App.Crosscutting.Commom.ViewModel.Core.Produto.Listar
{
    public class ListarProdutoRequest : PagedOptions
    {
        [StringLength(100)]
        public string? Nome { get; set; }

        public decimal? PrecoMin { get; set; }
        public decimal? PrecoMax { get; set; }
        public int? CategoriaId { get; set; }
    }
}
