#nullable enable
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppDonnyCuevas20210074.Models
{
    [Table("Alojamientos")]
    public class Alojamiento
    {
        [Key]
        public int IdAlojamiento { get; set; }

        [Required]
        public int IdLugar { get; set; }

        [StringLength(80)]
        public string? TipoAlojamiento { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal? PrecioMinimo { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal? PrecioMaximo { get; set; }

        public int? CantidadHabitaciones { get; set; }

        public TimeSpan? HoraEntrada { get; set; }

        public TimeSpan? HoraSalida { get; set; }

        [StringLength(500)]
        public string? EnlaceReserva { get; set; }

        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        public DateTime? FechaActualizacion { get; set; }

        [ForeignKey(nameof(IdLugar))]
        public virtual Lugar? Lugar { get; set; }
    }
}
