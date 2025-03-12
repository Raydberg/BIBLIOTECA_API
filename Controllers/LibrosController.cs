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

        public LibrosController (ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<LibroDTO>> Get ()
        {
            var libro = await context.Libros.ToListAsync();
            var libroDTO = _mapper.Map<IEnumerable<LibroDTO>>(libro);

            return libroDTO;
        }

        [HttpGet("{id:int}", Name = "ObtenerLibro")]
        public async Task<ActionResult<LibroDTO>> Get (int id)
        {
            var libro = await context.Libros
                .Include(libro => libro.Autor)
                .FirstOrDefaultAsync(libro => libro.Id == id);


            if (libro is Nullable)
            {
                return NotFound("Libro no encontrado");
            }
            var libroDTO = _mapper.Map<LibroDTO>(libro);

            return libroDTO;
        }

        [HttpPost]

        public async Task<ActionResult> Post (LibroCreateDTO libroCreateDto)
        {
            var libro = _mapper.Map<Libro>(libroCreateDto);

            // Permite obtener true o false , que cuumpla la condicion dada
            var isAutor = await context.Autores.AnyAsync(autor => autor.Id == libro.AutorId);
            if (!isAutor)
            {

                ModelState.AddModelError(nameof(libro.AutorId), "El autor de id {libro.AutorId} no existe");
                return ValidationProblem();
            }
            context.Add(libro);
            await context.SaveChangesAsync();

            var libroDto = _mapper.Map<LibroDTO>(libro);

            return CreatedAtRoute("ObtenerLibro", new { id = libro.Id }, libroDto);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put (int id, LibroCreateDTO libroCreateDto)
        {
            var libro = _mapper.Map<Libro>(libroCreateDto);

            var isAutor = await context.Autores.AnyAsync(autor => autor.Id == libro.AutorId);
            if (!isAutor)
            {
                return BadRequest($"El autor de id {libro.AutorId} no existe");
            }
            context.Update(libro);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Deleted (int id)
        {
            var libroBorrado = await context.Libros.Where(libro => libro.Id == id).ExecuteDeleteAsync();
            if (libroBorrado == 0)
            {
                return NotFound();
            }

            return Ok();
        }
    }
}
