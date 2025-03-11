using System.ComponentModel.DataAnnotations;

namespace BIBLIOTECA_API.Entidades
{
    public class Libro
    {
        public int Id { get; set; }
        [Required]
        public required string Titulo { get; set; }
        // Llave foranea para comunicarse con los autores
        public int AutorId { get; set; }


        //Propiedad de navegacion para ir a traer la data de la otra entidad
        public Autor? Autor { get; set; }

    }
}
