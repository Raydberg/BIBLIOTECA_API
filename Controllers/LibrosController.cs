using AutoMapper;
using BIBLIOTECA_API.DB;
using BIBLIOTECA_API.DTOs;
using BIBLIOTECA_API.Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BIBLIOTECA_API.Controllers
{
    [ApiController]
    [Route("api/libros")]
    public class LibrosController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper _mapper;

        public LibrosController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<LibroDTO>> Get()
        {
            var libro = await context.Libros.ToListAsync();
            var libroDTO = _mapper.Map<IEnumerable<LibroDTO>>(libro);

            return libroDTO;
        }

        [HttpGet("{id:int}", Name = "ObtenerLibro")]
        public async Task<ActionResult<LibroWithAutorsDTO>> Get(int id)
        {
            var libro = await context.Libros
                .Include(libro => libro.Autores)
                .ThenInclude(x=> x.Autor)
                .FirstOrDefaultAsync(libro => libro.Id == id);


            if (libro is Nullable)
            {
                return NotFound("Libro no encontrado");
            }

            var libroDTO = _mapper.Map<LibroWithAutorsDTO>(libro);

            return libroDTO;
        }

        [HttpPost]
        public async Task<ActionResult> Post(LibroCreateDTO libroCreateDto)
        {
            if (libroCreateDto.AutoresIds is null || libroCreateDto.AutoresIds.Count == 0)
            {
                // Retornar un error
                ModelState.AddModelError(nameof(libroCreateDto.AutoresIds), "No se puede crear un libro sin autores ");
                return ValidationProblem();
            }

            var autoresIdsExist = await context.Autores
                .Where(x => libroCreateDto.AutoresIds.Contains(x.Id))
                .Select(x => x.Id)
                .ToListAsync();

            if (autoresIdsExist.Count != libroCreateDto.AutoresIds.Count)
            {
                var autoresNotFound = libroCreateDto.AutoresIds.Except(autoresIdsExist);
                var autoresNotFoundString = string.Join(", ", autoresNotFound);
                var messageError = $"Los Siguientes autores no existes {autoresNotFoundString}";
                ModelState.AddModelError(nameof(libroCreateDto.AutoresIds), messageError);
                return ValidationProblem();
            }

            var libro = _mapper.Map<Libro>(libroCreateDto);

            context.Add(libro);
            await context.SaveChangesAsync();

            var libroDto = _mapper.Map<LibroDTO>(libro);
            AsignarOrdenAutores(libro);

            return CreatedAtRoute("ObtenerLibro", new { id = libro.Id }, libroDto);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, LibroCreateDTO libroCreateDto)
        {
            if (libroCreateDto.AutoresIds is null || libroCreateDto.AutoresIds.Count == 0)
            {
                // Retornar un error
                ModelState.AddModelError(nameof(libroCreateDto.AutoresIds), "No se puede crear un libro sin autores ");
                return ValidationProblem();
            }

            var autoresIdsExist = await context.Autores
                .Where(x => libroCreateDto.AutoresIds.Contains(x.Id))
                .Select(x => x.Id)
                .ToListAsync();

            if (autoresIdsExist.Count != libroCreateDto.AutoresIds.Count)
            {
                var autoresNotFound = libroCreateDto.AutoresIds.Except(autoresIdsExist);
                var autoresNotFoundString = string.Join(", ", autoresNotFound);
                var messageError = $"Los Siguientes autores no existes {autoresNotFoundString}";
                ModelState.AddModelError(nameof(libroCreateDto.AutoresIds), messageError);
                return ValidationProblem();
            }

            var libroDB = await context.Libros.Include(x => x.Autores)
                .FirstOrDefaultAsync(libro => libro.Id == id);
            if (libroDB is null)
            {
                return NotFound();
            }

            libroDB = _mapper.Map(libroCreateDto, libroDB);
            AsignarOrdenAutores(libroDB);
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Deleted(int id)
        {
            var libroBorrado = await context.Libros.Where(libro => libro.Id == id).ExecuteDeleteAsync();
            if (libroBorrado == 0)
            {
                return NotFound();
            }

            return NoContent();
        }

        private void AsignarOrdenAutores(Libro libro)
        {
            if (libro.Autores is not null)
            {
                for (int i = 0; i < libro.Autores.Count; i++)
                {
                    libro.Autores[i].Orden = i;
                }
            }
        }
    }
}