using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Store.App.Crosscutting.Commom.ViewModel.Core.Application.Usuario
{
    public class UsuarioEndereco
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
