namespace BIBLIOTECA_API.DTOs
{
    public class LibroWithAutorDTO : LibroDTO
    {
        public int AutorId { get; set; }
        public required string AutorNombre { get; set; }

    }
}
