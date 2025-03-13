using System.ComponentModel.DataAnnotations;

namespace BIBLIOTECA_API.DTOs
{
    public class CommentCreateDTO
    {
        [Required]
        public required string Cuerpo { get; set; }
    }
}
