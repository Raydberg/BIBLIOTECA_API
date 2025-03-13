using Microsoft.Build.Framework;

namespace BIBLIOTECA_API.Entidades
{
    public class Comment
    {
        // Guid -> String  aleatorio
        public Guid Id { get; set; }
        [Required]
        public required string Cuerpo { get; set; }
        public DateTime FechaPublicacion { get; set; }
        public int LibroId { get; set; }

        // Propiedad de Navegacion
        public Libro? Libro { get; set; }
    }
}
