#nullable enable
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppDonnyCuevas20210074.Models
{
    [Table("Rutas")]
    public class Ruta
    {
        [Key]
        public int IdRuta { get; set; }

        [Required]
        [StringLength(150)]
        public string Nombre { get; set; } = string.Empty;

        [Column(TypeName = "nvarchar(max)")]
        public string? Descripcion { get; set; }

        public int? IdLugarOrigen { get; set; }

        public int? IdLugarDestino { get; set; }

        [Column(TypeName = "decimal(8,2)")]
        public decimal? DistanciaKilometros { get; set; }

        public int? DuracionMinutos { get; set; }

        [StringLength(50)]
        public string? NivelDificultad { get; set; }

        [StringLength(80)]
        public string? TipoTransporte { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        public string? Instrucciones { get; set; }

        public bool Activa { get; set; } = true;

        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        public DateTime? FechaActualizacion { get; set; }

        [ForeignKey(nameof(IdLugarOrigen))]
        public virtual Lugar? LugarOrigen { get; set; }

        [ForeignKey(nameof(IdLugarDestino))]
        public virtual Lugar? LugarDestino { get; set; }
    }
}
