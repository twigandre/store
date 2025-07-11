﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Store.App.Core.Domain.Entitites
{
    [Table("usuario_endereco")]
    public class UsuarioEnderecoEntity
    {
        public virtual UsuarioEntity? Usuario { get; set; }

        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("cidade")]
        [StringLength(50)]
        [Required]
        public string Cidade { get; set; }

        [Column("logradouro")]
        [StringLength(50)]
        [Required]
        public string Logradouro { get; set; }

        [Column("numero")]
        [Required]
        public int NumeroImovel { get; set; }

        [Column("zipcode")]
        [StringLength(10)]
        [Required]
        public string ZipCode { get; set; }

        [Column("latitude")]
        [StringLength(15)]
        [Required]
        public string Latitude { get; set; }

        [Column("longitude")]
        [StringLength(15)]
        [Required]
        public string Longitude { get; set; }

        [Column("usuario_id")]
        [ForeignKey(nameof(Usuario))]
        [Required]
        public int UsuarioId { get; set; }

    }
}
