using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Store.App.Core.Application.Produto.Apagar
{
    public class ApagarProdutoCommand : IRequest<ApagarProdutoResponse>
    {
        [Required]
        public int Id { get; set; }
    }
}
