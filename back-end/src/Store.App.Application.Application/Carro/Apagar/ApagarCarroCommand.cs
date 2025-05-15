using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Store.App.Core.Application.Carro.Apagar
{
    public class ApagarCarroCommand : IRequest<ApagarCarroResult>
    {
        [Required]
        public int Id { get; set; }
    }
}
