using AutoMapper;
using BIBLIOTECA_API.DTOs;
using BIBLIOTECA_API.Entidades;

namespace BIBLIOTECA_API.Utils
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
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

            CreateMap<LibroCreateDTO, Libro>()
                .ForMember(ent => ent.Autores,
                    config => config.MapFrom(
                        dto => dto.AutoresIds
                            .Select(id => new AutorLibro { AutorId = id })
                    ));

            CreateMap<AutorLibro, LibroDTO>()
                .ForMember(dto => dto.id,config => config.MapFrom(ent => ent.LibroId))
                .ForMember(dto => dto.Titulo,config => config.MapFrom(ent => ent.Libro!.Titulo))
                ;

            CreateMap<Libro, LibroWithAutorsDTO>();

            
            
            CreateMap<AutorLibro, AutorDTO>()
                .ForMember(dto => dto.id,config=>config.MapFrom(ent => ent.AutorId))
                .ForMember(dto=> dto.NombreCompleto,config=>config.MapFrom(ent => MapperNameAndLastName(ent.Autor!)))
                ;

            CreateMap<LibroCreateDTO, AutorLibro>()
                .ForMember(ent => ent.Libro,
                    config => config.MapFrom(dto => new Libro{Titulo = dto.Titulo}));
                

            //----------------------------------------------------//


            CreateMap<CommentCreateDTO, Comment>();
            CreateMap<Comment, CommentDTO>()
                .ForMember(dto => dto.UserEmail,
                    config=> config.MapFrom(ent => ent.Usuario!.Email));
            CreateMap<CommentPatchDTO, Comment>().ReverseMap();
        }


        private string MapperNameAndLastName(Autor autor) => $"{autor.Nombre} {autor.Apellidos}";
    }
}