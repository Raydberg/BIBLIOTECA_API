using System.ComponentModel.DataAnnotations;

namespace BIBLIOTECA_API.Entidades
{
    public class Autor
    {
        public int Id { get; set; }
        //-> La propiedad es requerida
        [Required(ErrorMessage = "El campo {0} es requerido")] //-> uso de placeholder {0} que se reemplaza por el nombre del campo
        [StringLength(150, ErrorMessage = "El campo {0} debe tener {1} caracteres o menos")]
        public required string Nombre { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")] //-> uso de placeholder {0} que se reemplaza por el nombre del campo
        [StringLength(150, ErrorMessage = "El campo {0} debe tener {1} caracteres o menos")]
        public required string Apellidos { get; set; }
        [StringLength(20, ErrorMessage = "El campo {0} debe tener {1} caracteres o menos")]
        public string? Identificacion { get; set; }

        public List<AutorLibro> Libros { get; set; } = [];
    }
}
