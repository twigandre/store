using System.ComponentModel.DataAnnotations;

namespace Store.App.Crosscutting.Commom.ViewModel.Core.Usuario
{
    public class UsuarioEnderecoVM
    {
        public int? Id { get; set; }

        [StringLength(50)]
        [Required]
        public string Cidade { get; set; }

        [StringLength(50)]
        [Required]
        public string Logradouro { get; set; }

        [Required]
        public int NumeroImovel { get; set; }

        [StringLength(10)]
        [Required]
        public string ZipCode { get; set; }

        [StringLength(15)]
        [Required]
        public string Latitude { get; set; }

        [StringLength(15)]
        [Required]
        public string Longitude { get; set; }
    }
}
