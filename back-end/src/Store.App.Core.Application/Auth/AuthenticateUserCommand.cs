using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Store.App.Core.Application.Auth
{
    public class AuthenticateUserCommand : IRequest<AuthenticateUserResult>
    {
        [MinLength(3, ErrorMessage = "O email do usuário deve ter no mínimo 3 caracteres.")]
        [MaxLength(50, ErrorMessage = "O email do usuário deve ter no máximo 3 caracteres.")]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "O e-mail informado não é válido.")]
        [DataType(DataType.EmailAddress)]
        [Required]
        public string email { get; set; }

        [StringLength(200)]
        [Required]
        public string password { get; set; }
    }
}
