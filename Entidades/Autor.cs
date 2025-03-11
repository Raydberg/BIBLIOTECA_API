using System.ComponentModel.DataAnnotations;

namespace BIBLIOTECA_API.Entidades
{
    public class Autor
    {
        public int Id { get; set; }
        [Required] //-> La propiedad es requerida
        public required string Nombre { get; set; }


        //Propiedad de Navegacion - Traer Listado de lIbros
        public List<Libro> Libros { get; set; } = new List<Libro>();

    }
}
