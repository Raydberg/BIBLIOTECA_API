using System.ComponentModel.DataAnnotations;

namespace BIBLIOTECA_API.Entidades
{
    public class Libro
    {
        public int Id { get; set; }
        [Required]
        public required string Titulo { get; set; }

        public List<AutorLibro> Autores { get; set; } = [];

        public List<Comment> Comments { get; set; } = [];

    }
}
