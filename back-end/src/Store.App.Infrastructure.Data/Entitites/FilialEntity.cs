using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Store.App.Infrastructure.Database.Entities
{
    [Table("filial")]
    public class FilialEntity
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("nome")]
        [StringLength(100)]
        [Required]
        public string Nome { get; set; }
    }
}
