using MediatR;
using System.ComponentModel.DataAnnotations;


namespace Store.App.Core.Application.Usuario.Selecionar
{
    public class SelecionarUsuarioCommand : IRequest<SelecionarUsuarioResponse>
    {
        [Required]
        public int Id { get; set; }
    }
}
