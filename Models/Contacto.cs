#nullable enable
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppDonnyCuevas20210074.Models
{
    [Table("Contactos")]
    public class Contacto
    {
        [Key]
        public int IdContacto { get; set; }

        [Required]
        public int IdLugar { get; set; }

        [Required]
        [StringLength(30)]
        public string TipoContacto { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string ValorContacto { get; set; } = string.Empty;

        public bool EsPrincipal { get; set; } = false;

        [ForeignKey(nameof(IdLugar))]
        public virtual Lugar? Lugar { get; set; }
    }
}
