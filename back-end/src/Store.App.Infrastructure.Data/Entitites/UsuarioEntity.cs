using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Store.App.Infrastructure.Database.Entities
{
    [Table("usuario")]
    public class UsuarioEntity
    {
        public virtual FilialEntity? Filial { get; set; }

        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("nome")]
        [StringLength(100)]
        [Required]
        public string Nome { get; set; }

        [Column("email")]
        [StringLength(50)]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "O e-mail informado não é válido.")]
        [DataType(DataType.EmailAddress)]
        [Required]
        public string Email { get; set; }

        [Column("senha")]
        [StringLength(100)]
        [Required]
        public string Senha { get; set; }

        [Column("perfil")]
        [StringLength(20)]
        [RegularExpression(@"^(admin|vendedor)$", ErrorMessage = "O perfil deve ser 'admin' ou 'vendedor'.")]
        [Required]
        public string Perfil { get; set; }

        [Column("filial_id")]
        [ForeignKey(nameof(Filial))]
        public int? FilialId { get; set; }

        [Column("ativo")]
        [Required]
        public bool Ativo { get; set; }
    }
}
