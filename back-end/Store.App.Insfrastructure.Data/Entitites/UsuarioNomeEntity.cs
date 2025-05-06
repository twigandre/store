using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Store.App.Infrastructure.Database.Entities
{
    [Table("usuario_nome")]
    public class UsuarioNomeEntity
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("nome")]
        [StringLength(20)]
        [Required]
        public string Nome { get; set; }

        [Column("sobre_nome")]
        [StringLength(20)]
        [Required]
        public string SobreNome { get; set; }
    }
}
