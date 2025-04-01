using System.ComponentModel.DataAnnotations;
using BIBLIOTECA_API.Entidades;

namespace BIBLIOTECA_API.DTOs
{
    public class LibroCreateDTO
    {
        [Required]
        [StringLength(250, ErrorMessage = "El campo {0} debe tener {1} caracteres o mas ")]
        public required string Titulo { get; set; }

        public List<int> AutoresIds { get; set; } = [];
    }
}