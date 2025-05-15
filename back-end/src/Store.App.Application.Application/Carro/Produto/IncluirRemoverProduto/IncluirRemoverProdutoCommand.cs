using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Store.App.Core.Application.Carro.Produto.IncluirRemoverProduto
{
    public class IncluirRemoverProdutoCommand : IRequest<IncluirRemoverProdutoResult>
    {
        [Required(ErrorMessage = "O campo [IdProduto] é obrigatório.")]
        public int IdProduto { get; set; }

        [Required]
        [RegularExpression(@"^(incluir|remover)$", ErrorMessage = "O status deve ser 'ativo','inativo' ou 'suspenso'.")]
        public string tipoOperacao { get; set; }
    }
}
