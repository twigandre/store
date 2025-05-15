using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Store.App.Core.Domain.Entitites
{
    [Table("vendas")]
    public class VendaEntity
    {
        public virtual UsuarioEntity Usuario { get; set; }
        public virtual FilialEntity Filial { get; set; }
        public virtual CarroEntity Carro { get; set; }
        public virtual ICollection<VendaItensEntity> ItensVenda { get; set; }

        public VendaEntity()
        {
            ItensVenda = new HashSet<VendaItensEntity>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("numero_venda")]
        [StringLength(50)]
        [Required]
        public string NumeroVenda { get; set; }

        [Column("data_venda")]
        [Required]
        public DateTime DataHoraVenda { get; set; }

        [Column("usuario_id")]
        [ForeignKey(nameof(Usuario))]
        [Required]
        public int UsuarioId { get; set; }

        [Column("filial_id")]
        [ForeignKey(nameof(Filial))]
        [Required]
        public int FilialId { get; set; }

        [Column("carro_id")]
        [ForeignKey(nameof(Carro))]
        [Required]
        public int CarroId { get; set; }

        [Column("valor_total")]
        [Required]
        public decimal ValorTotal { get; set; }

        [Column("is_cancelada")]
        [Required]
        public bool IsCancelada { get; set; }
    }
}
