using System;

namespace Aplicacion.DTO
{
    public class InstructorDTO
    {
        public Guid InstructorId { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Grado { get; set; }
        public byte[] FotoPerfil { get; set; }
    }
}