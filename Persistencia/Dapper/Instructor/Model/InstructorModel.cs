using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistencia.Dapper.Instructor.Model
{
    public class InstructorModel
    {
        public Guid InstructorId { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Grado { get; set; }
    }
}
