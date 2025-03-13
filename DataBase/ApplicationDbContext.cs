using BIBLIOTECA_API.Entidades;
using Microsoft.EntityFrameworkCore;

namespace BIBLIOTECA_API.DB
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext (DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating (ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); //->Nunca se borra

            // Alternativa a poner [StringLength(150)]
            //modelBuilder.Entity<Autor>().Property(autor => autor.Nombre).HasMaxLength(150);

        }


        // Se crea la tabla en la base de datos con el nombre de autores con las propiedades de la clase
        public DbSet<Autor> Autores { get; set; }
        public DbSet<Libro> Libros { get; set; }

        public DbSet<Comment> Comments { get; set; }

    }
}
