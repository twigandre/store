using Store.App.Crosscutting.Commom.ViewModel.Core.Categoria;
using Store.App.Crosscutting.Commom.ViewModel.Core.Produto;

namespace Store.App.Crosscutting.Commom.ViewModel.Core.Listar
{
    public class ListarProdutoResult
    {
        public int? Id { get; set; }

        public string Nome { get; set; }

        public decimal PrecoUnitario { get; set; }

        public CategoriaVM Categoria { get; set; }

        public int CategoriaId { get; set; }

        public string? Descricao { get; set; }

        public ArquivoVM Imagem { get; set; }

        public AvaliacaoVM Avaliacao { get; set; }
    }
}
