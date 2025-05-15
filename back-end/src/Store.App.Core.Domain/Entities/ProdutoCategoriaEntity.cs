using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Store.App.Core.Domain.Entitites
{
    [Table("produto_categoria")]
    public class ProdutoCategoriaEntity
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("nome")]
        [StringLength(150)]
        [Required]
        public string Nome { get; set; }
    }
}
