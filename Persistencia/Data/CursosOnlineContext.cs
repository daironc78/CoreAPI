using Dominio.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistencia.Data
{
    public class CursosOnlineContext : IdentityDbContext<Usuario>
    {
        public CursosOnlineContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<CursoInstructor>().HasKey(ci => new { ci.InstructorId, ci.CursoId });
        }

        public DbSet<Comentario> Comentarios { get; set; }
        public DbSet<Curso> Cursos { get; set; }
        public DbSet<CursoInstructor> CursoInstructors { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Precio> Precios { get; set; }
    }
}