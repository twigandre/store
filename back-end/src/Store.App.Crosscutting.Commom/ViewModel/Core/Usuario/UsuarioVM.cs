using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.App.Crosscutting.Commom.ViewModel.Core.Usuario
{
    public class UsuarioVM
    {
        public int Id { get; set; }
        public string PrimeiroNome { get; set; }
        public string SobreNome { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public string Senha { get; set; }
        public string Perfil { get; set; }
        public int? FilialId { get; set; }
        public string Status { get; set; }
    }
}
