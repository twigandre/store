using System.ComponentModel.DataAnnotations;

namespace Store.App.Crosscutting.Commom.ViewModel.Login
{
    public class LoginRequest
    {
        [StringLength(50)]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "O e-mail informado não é válido.")]
        [DataType(DataType.EmailAddress)]
        [Required]
        public string username { get; set; }

        [StringLength(200)]
        [Required]
        public string password { get; set; }
    }
}
