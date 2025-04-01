namespace BIBLIOTECA_API.DTOs
{
    public class LibroWithAutorsDTO : LibroDTO
    {
        public List<AutorDTO> Autores { get; set; }
    }
}
