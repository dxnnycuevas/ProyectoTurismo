#nullable enable
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppDonnyCuevas20210074.Models
{
    [Table("Atractivos")]
    public class Atractivo
    {
        [Key]
        public int IdAtractivo { get; set; }

        [Required]
        public int IdLugar { get; set; }

        [StringLength(80)]
        public string? TipoAtractivo { get; set; }

        [StringLength(100)]
        public string? EstadoConservacion { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        public string? InformacionNatural { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        public string? InformacionCultural { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        public string? InformacionAcceso { get; set; }

        public int? DuracionVisitaMinutos { get; set; }

        [StringLength(50)]
        public string? NivelDificultad { get; set; }

        public bool Destacado { get; set; } = false;

        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        public DateTime? FechaActualizacion { get; set; }

        [ForeignKey(nameof(IdLugar))]
        public virtual Lugar? Lugar { get; set; }
    }
}
