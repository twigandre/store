using Store.App.Crosscutting.Commom.ViewModel.Core.Application.Usuario;
using Store.App.Crosscutting.Commom.ViewModel.Core.Application;

namespace Store.App.Core.Application.Usuario.Selecionar
{
    public class SelecionarUsuarioResponse
    {
        public int? Id { get; set; }

        public UsuarioNome Nome { get; set; }

        public UsuarioEndereco Endereco { get; set; }

        public string Email { get; set; }

        public string Telefone { get; set; }

        public string? Senha { get; set; }

        public string Perfil { get; set; }

        public string Status { get; set; }

        public int? FilialId { get; set; }

        public Filial Filial { get; set; }
    }
}
