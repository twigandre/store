using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Store.App.Core.Domain.Entitites
{
    [Table("produtos")]
    public class ProdutoEntity
    {
        public virtual ProdutoCategoriaEntity? Categoria { get; set; }
        public virtual ICollection<CarroEntity> Carro { get; set; }

        public ProdutoEntity()
        {
            Carro = new HashSet<CarroEntity>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("nome")]
        [StringLength(100)]
        [Required]
        public string Nome { get; set; }

        [Column("preco_unitario")]        
        [Required]
        public decimal PrecoUnitario { get; set; }

        [Column("categoria_id")]
        [ForeignKey(nameof(Categoria))]
        [Required]
        public int CategoriaId { get; set; }

        [Column("descricao")]
        [StringLength(500)]
        public string? Descricao { get; set; }

        [Column("nota_produto")]
        [Range(0, 5, ErrorMessage = "O valor deve estar entre 0 e 5.")]
        [Required]
        public int NotaProduto { get; set; }

        [Column("quantidade_vendas")]
        [Required]
        public int QuantidadeVendas { get; set; }
    }
}
