using Aplicacion.DTO;
using AutoMapper;
using System.Linq;

namespace Aplicacion.Automapper
{
    public class AutomaperProfile : Profile
    {
        public AutomaperProfile()
        {
            CreateMap<Dominio.Models.Curso, CursoDTO>()
                .ForMember(x => x.Instructores, y => y.MapFrom(z => z.InstructorLink.Select(a => a.Instructor).ToList()))
                .ForMember(x => x.Comentarios, y => y.MapFrom(z => z.ComentarioLista))
                .ForMember(x => x.Precio, y => y.MapFrom(z => z.PrecioPromocion));
            CreateMap<Dominio.Models.CursoInstructor, CursoInstructorDTO>();
            CreateMap<Dominio.Models.Instructor, InstructorDTO>();
            CreateMap<Dominio.Models.Comentario, ComentarioDTO>();
            CreateMap<Dominio.Models.Precio, PrecioDTO>();
        }
    }
}
