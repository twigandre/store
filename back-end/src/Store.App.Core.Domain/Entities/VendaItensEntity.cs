using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Store.App.Core.Domain.Entitites
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

        [Column("preco_unitario")]
        [Required]
        public decimal PrecoUnitario { get; set; }

        [Column("desconto")]
        [Required]
        public decimal Desconto { get; set; }

        [Column("valor_total")]
        [Required]
        public decimal ValorTotal { get; set; }
    }
}
