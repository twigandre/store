using MediatR;
using Store.App.Crosscutting.Commom.ViewModel.Core.Application.Usuario;
using Store.App.Crosscutting.Commom.ViewModel.Core.Application.Usuario.Salvar;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Store.App.Core.Application.Usuario.Salvar
{
    public class SalvarUsuarioCommand : IRequest<SalvarUsuarioResponse>
    {
        public int? Id { get; set; }

        [Required]
        public UsuarioNome Nome { get; set; }

        [Required]
        public UsuarioEndereco Endereco { get; set; }

        [StringLength(50)]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "O e-mail informado não é válido.")]
        [DataType(DataType.EmailAddress)]
        [Required]
        public string Email { get; set; }

        [StringLength(15)]
        [DataType(DataType.PhoneNumber)]
        [Required]
        public string Telefone { get; set; }
                
        [StringLength(200)]
        [AllowNull]
        public string? Senha { get; set; }

        [StringLength(20)]
        [RegularExpression(@"^(cliente|gerente|administrador)$", ErrorMessage = "O perfil deve ser 'cliente','gerente' ou 'administrador'.")]
        [Required]
        public string Perfil { get; set; }

        [RegularExpression(@"^(ativo|inativo|suspenso)$", ErrorMessage = "O status deve ser 'ativo','inativo' ou 'suspenso'.")]
        [Required]
        public string Status { get; set; }

        [Required]
        public int FilialId { get; set; }
    }
}
