using MediatR;
using Microsoft.EntityFrameworkCore;
using Store.App.Crosscutting.Commom.ViewModel;
using System.ComponentModel.DataAnnotations;

namespace Store.App.Core.Application.Produto.Salvar
{
    public class SalvarProdutoCommand : IRequest<SalvarProdutoResponse>
    {
        public int? Id { get; set; }

        [StringLength(100)]
        [Required]
        public string Nome { get; set; }

        [Required]
        public decimal PrecoUnitario { get; set; }

        [Required]
        public int CategoriaId { get; set; }

        [StringLength(500)]
        public string? Descricao { get; set; }

        [Required]
        public ArquivoVM Imagem { get; set; }
    }
}
