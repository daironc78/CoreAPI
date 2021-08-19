using System;
using System.Collections.Generic;

namespace Dominio.Models
{
    public class Curso
    {
        public Guid CursoId { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public DateTime? FechaPublicacion { get; set; }
        public byte[] FotoPortada { get; set; }
        public DateTime? FechaCreacion { get; set; }
        // UNO A UNO
        public Precio PrecioPromocion { get; set; }
        // UNO A MUCHOS
        public ICollection<Comentario> ComentarioLista { get; set; }
        // MUCHOS A MUCHOS
        public ICollection<CursoInstructor> InstructorLink { get; set; }
    }
}
