using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Store.App.Core.Application.Usuario.Apagar
{
    public class ApagarUsuarioCommand : IRequest<ApagarUsuarioResponse>
    {
        [Required]
        public int Id { get; set; }
    }
}
