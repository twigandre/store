using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Store.App.Core.Application.Carro.Selecionar
{
    public class SelecionarCarroCommand : IRequest<SelecionarCarroResult>
    {
        [Required]
        public int Id { get; set; }
    }
}
