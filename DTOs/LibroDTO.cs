using BIBLIOTECA_API.Entidades;

namespace BIBLIOTECA_API.DTOs
{
    public class LibroDTO
    {
        public int id { get; set; }
        public required string Titulo { get; set; }
        public int AutorId { get; set; }
        public Autor? Autor { get; set; }
    }
}
