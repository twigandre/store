using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Store.App.Infrastructure.Database.Entities
{
    [Table("clientes")]
    public class ClienteEntity
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("nome")]
        [StringLength(100)]
        [Required]
        public string Nome { get; set; }

        [Column("cpf")]
        [StringLength(11)]
        [Required]
        public string Cpf { get; set; }

        [Column("telefone")]
        [StringLength(15)]
        [Required]
        public string telefone { get; set; }

        [Column("email")]
        [StringLength(50)]
        [Required]
        public string email { get; set; }
    }
}
