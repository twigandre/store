using Store.App.Infrastructure.Database.DbEntities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Store.App.Infrastructure.Database.DbEntities
{
    [Table("carro")]
    public class CarroEntity
    {
        public virtual UsuarioEntity Usuario { get; set; }
        public virtual ICollection<CarroProdutoEntity> Produtos { get; set; }

        public CarroEntity()
        {
            Produtos = new HashSet<CarroProdutoEntity>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("usuario_id")]
        [ForeignKey(nameof(Usuario))]
        [Required]
        public int UsuarioId { get; set; }

        [Column("data_criacao")]
        public DateTime DataCriacao { get; set; }

    }
}
