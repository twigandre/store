using Store.App.Crosscutting.Commom.ViewModel.Core.Usuario;

namespace Store.App.Crosscutting.Commom.ViewModel.Core.Carro.Listar
{
    public class ListarCarroResult
    {
        public UsuarioVM Comprador { get; set; }

        public List<CarroProdutoVM> CarroProduto { get; set; }

        public int Id { get; set; }

        public int UsuarioId { get; set; }

        public DateTime DataCriacao { get; set; }

        public string Status { get; set; }
    }
}
