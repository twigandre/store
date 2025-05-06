using Store.App.Infrastructure.Database.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Store.App.Infrastructure.Database.Entities
{
    [Table("carro")]
    public class CarroEntity
    {
        public virtual ClienteEntity Cliente { get; set; }
        public virtual ICollection<CarroProdutoEntity> Produtos { get; set; }

        public CarroEntity()
        {
            Produtos = new HashSet<CarroProdutoEntity>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("cliente_id")]
        [ForeignKey(nameof(Cliente))]
        [Required]
        public int ClienteId { get; set; }

        [Column("data_criacao")]
        public DateTime DataCriacao { get; set; }

    }
}
