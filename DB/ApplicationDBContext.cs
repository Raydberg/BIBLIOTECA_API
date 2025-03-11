using BIBLIOTECA_API.Entidades;
using Microsoft.EntityFrameworkCore;

namespace BIBLIOTECA_API.DB
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext (DbContextOptions options) : base(options)
        {

        }

        // Se crea la tabla en la base de datos con el nombre de autores con las propiedades de la clase
        public DbSet<Autor> Autores { get; set; }
        public DbSet<Libro> Libros { get; set; }

    }
}
