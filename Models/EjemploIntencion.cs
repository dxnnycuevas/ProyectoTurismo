#nullable enable
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppDonnyCuevas20210074.Models
{
    [Table("EjemplosIntencion")]
    public class EjemploIntencion
    {
        [Key]
        public int IdEjemplo { get; set; }

        [Required]
        public int IdIntencion { get; set; }

        [Required]
        [StringLength(1000)]
        public string Texto { get; set; } = string.Empty;

        [Required]
        [StringLength(10)]
        public string Idioma { get; set; } = "es";

        [ForeignKey(nameof(IdIntencion))]
        public virtual Intencion? Intencion { get; set; }
    }
}
