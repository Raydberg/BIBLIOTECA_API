﻿
namespace BIBLIOTECA_API.DTOs
{
    public class CommentDTO
    {
        public Guid Id { get; set; }
        public required string Cuerpo { get; set; }
        public DateTime FechaPublicacion { get; set; }
    }
}
