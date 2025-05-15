using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Store.App.Core.Application.Carro.Produto.RemoverProduto
{
    public class RemoverProdutoCommand : IRequest<RemoverProdutoResult>
    {
        [Required(ErrorMessage = "O campo [IdProduto] é obrigatório.")]
        public int IdProduto { get; set; }
    }
}
