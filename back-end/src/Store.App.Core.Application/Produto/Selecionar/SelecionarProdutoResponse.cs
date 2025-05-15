using Store.App.Crosscutting.Commom.ViewModel.Core.Categoria;
using Store.App.Crosscutting.Commom.ViewModel.Core.Produto;
using Store.App.Crosscutting.Commom.ViewModel;

namespace Store.App.Core.Application.Produto.Selecionar
{
    public class SelecionarProdutoResponse
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
