using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dominio.Models
{
    public class Precio
    {
        public Guid PrecioId { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal PrecioActual { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Promocion { get; set; }
        public Guid CursoId { get; set; }
        // UNO A UNO
        public Curso Curso { get; set; }
    }
}
