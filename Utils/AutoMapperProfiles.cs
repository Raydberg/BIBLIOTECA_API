using AutoMapper;
using BIBLIOTECA_API.DTOs;
using BIBLIOTECA_API.Entidades;

namespace BIBLIOTECA_API.Utils
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles ()
        {
            // Desde Autor hacia AutorDTO
            // Autor -> AutorDTO
            CreateMap<Autor, AutorDTO>()
                .ForMember(
                    dto => dto.NombreCompleto,
                    config => config.MapFrom(autor => $"{autor.Nombre} {autor.Apellidos}")
                    );



            CreateMap<AutorCreateResponseDTO, Autor>();


            CreateMap<Libro, LibroDTO>();

            CreateMap<LibroCreateDTO, Libro>();
        }
    }
}
