using System.ComponentModel.DataAnnotations;

namespace Store.App.Crosscutting.Commom.ViewModel.Core.Application.Usuario
{
    public class UsuarioNome
    {
        [StringLength(20)]
        [Required]
        public string PrimeiroNome { get; set; }

        [StringLength(20)]
        [Required]
        public string SobreNome { get; set; }
    }
}
