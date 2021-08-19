using System;

namespace Aplicacion.DTO
{
    public class PrecioDTO
    {
        public Guid PrecioId { get; set; }
        public decimal PrecioActual { get; set; }
        public decimal Promocion { get; set; }
        public Guid CursoId { get; set; }
    }
}
