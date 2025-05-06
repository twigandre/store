using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Store.App.Infrastructure.Database.Entities
{
    [Table("usuario")]
    public class UsuarioEntity
    {
        public virtual FilialEntity? Filial { get; set; }
        public virtual UsuarioNomeEntity? Nome { get; set; }
        public virtual UsuarioEnderecoEntity? Endereco { get; set; }

        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("email")]
        [StringLength(50)]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "O e-mail informado não é válido.")]
        [DataType(DataType.EmailAddress)]
        [Required]
        public string Email { get; set; }

        [Column("phone")]
        [StringLength(15)]
        [DataType(DataType.PhoneNumber)]
        [Required]
        public string Telefone { get; set; }

        [Column("senha")]
        [StringLength(200)]
        [Required]
        public string Senha { get; set; }

        [Column("perfil")]
        [StringLength(20)]
        [RegularExpression(@"^(cliente|gerente|administrador)$", ErrorMessage = "O perfil deve ser 'cliente','gerente' ou 'administrador'.")]
        [Required]
        public string Perfil { get; set; }

        [Column("filial_id")]
        [ForeignKey(nameof(Filial))]
        public int? FilialId { get; set; }

        [Column("nome_id")]
        [ForeignKey(nameof(Nome))]
        [Required]
        public int NomeId { get; set; }

        [Column("endereco_id")]
        [ForeignKey(nameof(Endereco))]
        [Required]
        public int EnderecoId { get; set; }

        [Column("status")]
        [RegularExpression(@"^(ativo|inativo|suspenso)$", ErrorMessage = "O status deve ser 'ativo','inativo' ou 'suspenso'.")]
        [Required]
        public string Status { get; set; }
    }
}
