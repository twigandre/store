using Store.App.Crosscutting.Commom.ViewModel.Core.Filial;

namespace Store.App.Crosscutting.Commom.ViewModel.Core.Usuario.Listar
{
    public class ListarUsuarioResult
    {
        public int? Id { get; set; }

        public UsuarioNomeVM Nome { get; set; }

        public UsuarioEnderecoVM Endereco { get; set; }

        public string Email { get; set; }

        public string Telefone { get; set; }

        public string? Senha { get; set; }

        public string Perfil { get; set; }

        public string Status { get; set; }

        public int? FilialId { get; set; }

        public FilialVM Filial { get; set; }
    }
}
