using Store.App.Infrastructure.Database.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Store.App.Infrastructure.Database.Entities
{
    [Table("carro_produtos")]
    public class CarroProdutoEntity
    {
        public virtual CarroEntity Carro { get; set; }
        public virtual ProdutoEntity Produto { get; set; }

        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("carro_id")]
        [ForeignKey(nameof(Carro))]
        [Required]
        public int CarroId { get; set; }

        [Column("produto_id")]
        [ForeignKey(nameof(Produto))]
        [Required]
        public int ProdutoId { get; set; }

        [Column("quantidade")]
        [Required]
        public int Quantidade { get; set; }
    }
}
