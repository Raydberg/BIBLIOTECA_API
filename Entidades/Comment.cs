namespace BIBLIOTECA_API.Entidades
{
    public class Comment
    {
        // Guid -> String  aleatorio
        public Guid Id { get; set; }
        public required string cuerpo { get; set; }
        public DateTime FechaPublicacion { get; set; }
        public int LibroId { get; set; }

        // Propiedad de Navegacion
        public Libro? Libro { get; set; }
    }
}
