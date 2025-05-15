using Store.App.Crosscutting.Commom.ViewModel.Core.Carro;
using Store.App.Crosscutting.Commom.ViewModel.Core.Usuario;

namespace Store.App.Core.Application.Carro.Selecionar
{
    public class SelecionarCarroResult
    {
        public UsuarioVM Comprador { get; set; }

        public List<CarroProdutoVM> CarroProduto { get; set; }

        public int Id { get; set; }

        public int UsuarioId { get; set; }

        public DateTime DataCriacao { get; set; }

        public string Status { get; set; }
    }
}
