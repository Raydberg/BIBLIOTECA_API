using BIBLIOTECA_API.Entidades;

namespace BIBLIOTECA_API.DTOs
{
    public class LibroCreateDTO
    {
        public required string Titulo { get; set; }
        public int AutorId { get; set; }
        public Autor? Autor { get; set; }
    }
}
