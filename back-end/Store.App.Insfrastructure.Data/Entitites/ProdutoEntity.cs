using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Store.App.Infrastructure.Database.Entities
{
    [Table("produtos")]
    public class ProdutoEntity
    {
        public virtual ProdutoCategoriaEntity? Categoria { get; set; }

        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("nome")]
        [StringLength(100)]
        [Required]
        public string Nome { get; set; }

        [Precision(10, 2)]
        [Column("preco_unitario")]        
        [Required]
        public decimal PrecoUnitario { get; set; }

        [Column("categoria_id")]
        [ForeignKey(nameof(Categoria))]
        [Required]
        public int CategoriaId { get; set; }
    }
}
