#nullable enable
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppDonnyCuevas20210074.Models
{
    [Table("Horarios")]
    public class Horario
    {
        [Key]
        public int IdHorario { get; set; }

        [Required]
        public int IdLugar { get; set; }

        // 1 = lunes ... 7 = domingo (coincide con el CHECK del script original)
        [Required]
        [Range(1, 7)]
        public byte DiaSemana { get; set; }

        public TimeSpan? HoraApertura { get; set; }

        public TimeSpan? HoraCierre { get; set; }

        public bool Cerrado { get; set; } = false;

        [ForeignKey(nameof(IdLugar))]
        public virtual Lugar? Lugar { get; set; }
    }
}
