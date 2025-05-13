using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Store.App.Infrastructure.Database.DbEntities
{
    [Table("itens_venda")]
    public class VendaItensEntity
    {
        public virtual VendaEntity Venda { get; set; }
        public virtual ProdutoEntity Produto { get; set; }

        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("venda_id")]
        [ForeignKey(nameof(Venda))]
        [Required]
        public int VendaId { get; set; }

        [Column("produto_id")]
        [ForeignKey(nameof(Produto))]
        [Required]
        public int ProdutoId { get; set; }

        [Column("quantidade")]
        [Required]
        public int Quantidade { get; set; }

        [Precision(10, 2)]
        [Column("preco_unitario")]
        [Required]
        public decimal PrecoUnitario { get; set; }

        [Precision(10, 2)]
        [Column("desconto")]
        [Required]
        public decimal Desconto { get; set; }

        [Precision(12, 2)]
        [Column("valor_total")]
        [Required]
        public decimal ValorTotal { get; set; }

        [Column("is_cancelado")]
        [Required]
        public bool IsCancelado { get; set; }
    }
}
