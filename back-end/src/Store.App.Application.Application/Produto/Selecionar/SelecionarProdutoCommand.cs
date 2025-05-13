using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Store.App.Core.Application.Produto.Selecionar
{
    public class SelecionarProdutoCommand : IRequest<SelecionarProdutoResponse>
    {
        [Required]
        public int Id { get; set; }
    }
}
