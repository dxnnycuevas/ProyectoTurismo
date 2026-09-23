#nullable enable
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppDonnyCuevas20210074.Models
{
    [Table("Intenciones")]
    public class Intencion
    {
        [Key]
        public int IdIntencion { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Descripcion { get; set; }

        public bool Activa { get; set; } = true;

        public virtual ICollection<EjemploIntencion> Ejemplos { get; set; } = new List<EjemploIntencion>();
    }
}
