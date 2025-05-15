using Store.App.Core.Domain.Entitites;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Store.App.Core.Domain.Entitites
{
    [Table("carro")]
    public class CarroEntity
    {
        public virtual UsuarioEntity Usuario { get; set; }
        public virtual ICollection<CarroProdutoEntity> CarroProduto { get; set; }

        public CarroEntity()
        {
            CarroProduto = new HashSet<CarroProdutoEntity>();
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

        [Column("status")]
        [RegularExpression(@"(em_andamento|cancelado|compra_realizada)$", ErrorMessage = "O status deve ser 'em_andamento','cancelado' ou 'compra_realizada'.")]
        [Required]
        public string Status { get; set; }

    }
}
