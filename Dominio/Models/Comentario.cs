using System;

namespace Dominio.Models
{
    public class Comentario
    {
        public Guid ComentarioID { get; set; }
        public string Alumno { get; set; }
        public int Puntaje { get; set; }
        public string ComentarioTexto { get; set; }
        public Guid CursoId { get; set; }
        public DateTime? FechaCreacion { get; set; }
        // UNO A MUCHOS
        public Curso Curso { get; set; }
    }
}
