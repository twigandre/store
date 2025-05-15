using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Store.App.Core.Domain.Entitites
{
    [Table("usuario")]
    public class UsuarioEntity
    {
        public virtual FilialEntity? Filial { get; set; }
        public virtual ICollection<UsuarioEnderecoEntity> Endereco { get; set; } = new List<UsuarioEnderecoEntity>();
        public virtual ICollection<CarroEntity> Carro { get; set; }
        public UsuarioEntity()
        {
            Endereco = new HashSet<UsuarioEnderecoEntity>();
            Carro = new HashSet<CarroEntity>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("nome")]
        [StringLength(20)]
        [Required]
        public string PrimeiroNome { get; set; }

        [Column("sobre_nome")]
        [StringLength(20)]
        [Required]
        public string SobreNome { get; set; }

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

        [Column("status")]
        [RegularExpression(@"^(ativo|inativo|suspenso)$", ErrorMessage = "O status deve ser 'ativo','inativo' ou 'suspenso'.")]
        [Required]
        public string Status { get; set; }
    }
}
