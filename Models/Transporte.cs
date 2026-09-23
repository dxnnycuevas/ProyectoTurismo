#nullable enable
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppDonnyCuevas20210074.Models
{
    [Table("Transportes")]
    public class Transporte
    {
        [Key]
        public int IdTransporte { get; set; }

        [Required]
        public int IdLugar { get; set; }

        [Required]
        [StringLength(80)]
        public string TipoTransporte { get; set; } = string.Empty;

        [StringLength(500)]
        public string? ZonaCobertura { get; set; }

        [StringLength(500)]
        public string? Horario { get; set; }

        [StringLength(500)]
        public string? InformacionPrecio { get; set; }

        public bool RequiereReserva { get; set; } = false;

        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        public DateTime? FechaActualizacion { get; set; }

        [ForeignKey(nameof(IdLugar))]
        public virtual Lugar? Lugar { get; set; }
    }
}
