#nullable enable
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppDonnyCuevas20210074.Models
{
    [Table("Restaurantes")]
    public class Restaurante
    {
        [Key]
        public int IdRestaurante { get; set; }

        [Required]
        public int IdLugar { get; set; }

        [StringLength(100)]
        public string? TipoComida { get; set; }

        public byte? NivelPrecio { get; set; }

        public TimeSpan? HoraApertura { get; set; }

        public TimeSpan? HoraCierre { get; set; }

        [StringLength(500)]
        public string? EnlaceMenu { get; set; }

        public bool ServicioDomicilio { get; set; } = false;

        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        public DateTime? FechaActualizacion { get; set; }

        [ForeignKey(nameof(IdLugar))]
        public virtual Lugar? Lugar { get; set; }
    }
}
