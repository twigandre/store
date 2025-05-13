using Store.App.Core.Domain.Validation;
using System.ComponentModel.DataAnnotations;

namespace Store.App.Crosscutting.Commom.ViewModel
{
    public class ArquivoVM
    {
        [Required]
        [StringLength(100)]
        public string Nome { get; set; }

        [Required]
        [Base64Image(ErrorMessage = "A imagem precisa estar em formato Base64 válido.")]
        public string Hash { get; set; }
    }
}
