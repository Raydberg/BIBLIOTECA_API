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
                    config => config.MapFrom(autor => MapperNameAndLastName(autor))
                    );

            CreateMap<Autor, AutorWithLibrosDTO>()
                .ForMember(
                    dto => dto.NombreCompleto,
                    config => config.MapFrom(autor => MapperNameAndLastName(autor))
                );

            CreateMap<AutorCreateResponseDTO, Autor>();

            CreateMap<Autor, AutorPatchDTO>();
            // Lo contrario
            CreateMap<Autor, AutorPatchDTO>().ReverseMap();



            //----------------------------------------------------------------//



            CreateMap<Libro, LibroDTO>();

            CreateMap<LibroCreateDTO, Libro>();

            CreateMap<Libro, LibroWithAutorDTO>().ForMember(dto => dto.AutorNombre
                , config => config.MapFrom(entidad => MapperNameAndLastName(entidad.Autor!)));


            //----------------------------------------------------//


            CreateMap<CommentCreateDTO, Comment>();
            CreateMap<CommentPatchDTO, Comment>();
            CreateMap<CommentPatchDTO, Comment>().ReverseMap();

        }


        private string MapperNameAndLastName (Autor autor) => $"{autor.Nombre} {autor.Apellidos}";
    }
}
