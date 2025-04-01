using AutoMapper;
using BIBLIOTECA_API.DB;
using BIBLIOTECA_API.DTOs;
using BIBLIOTECA_API.Entidades;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BIBLIOTECA_API.Controllers
{
    [ApiController] //-> Se refiere a un controlador 
    [Route("api/autores")] //-> Ruta en la que se encuentra el controlador 
    public class AutoresController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper _mapper;

        private readonly ILogger<AutoresController> logger;
        // Inyeccion de dependencia para nuestro ApplicationDbContext
        public AutoresController (ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IEnumerable<AutorDTO>> Get ()
        {
            var autores = await context.Autores.ToListAsync();

            // Usamos nuestro mapper 
            // Dentro del Map<T> -> Ponemos la entidad o el resultado que vendra para mostrar 
            var autoresDTO = _mapper.Map<IEnumerable<AutorDTO>>(autores);
            return autoresDTO;
        }



        // Si vamos a usar dos metodos GET tenemos que diferenciarlos
        // En este caso vamos a llamar mediante el ID y tendria que ir asi como estan juntos

        /**
         * El ActionResult como su nombre lo indica es el resultado de una action
         * siempre que vayamos a enviar una data para esperar algo a cambio es
         * conveniente usarlo
         */
        [HttpGet("{id:int}", Name = "ObtenerAutor")]
        public async Task<ActionResult<AutorWithLibrosDTO>> Get (int id)
        {
            var autor = await context.Autores
                .Include(autor => autor.Libros)
                .ThenInclude(X=>X.Libro)
                .FirstOrDefaultAsync(autor => autor.Id == id);

            if (autor is null)
            {
                return NotFound();
            }
            // Hacemos nuestro mappeo
            var autorDto = _mapper.Map<AutorWithLibrosDTO>(autor);

            return autorDto;
        }

        [HttpPost]
        public async Task<ActionResult> Post (AutorCreateResponseDTO autorCreateDto)
        {
            var autor = _mapper.Map<Autor>(autorCreateDto);
            context.Autores.Add(autor);
            // Guardar cambios de manera asincrona
            await context.SaveChangesAsync();
            var autorDto = _mapper.Map<AutorDTO>(autor);
            return CreatedAtRoute("ObtenerAutor", new { id = autor.Id }, autorDto);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put (int id, AutorCreateResponseDTO autorCreateResponse)
        {
            var autor = _mapper.Map<Autor>(autorCreateResponse);
            context.Update(autorCreateResponse);
            autor.Id = id;
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPatch("{id:int}")]
        public async Task<ActionResult> Patch (int id, JsonPatchDocument<AutorPatchDTO> patchDoc)
        {
            if (patchDoc is null) { return BadRequest(); }

            var autorDB = await context.Autores.FirstOrDefaultAsync(autor => autor.Id == id);
            if (autorDB is null) { return NotFound(); }

            // Mapeo
            var autorPatchDto = _mapper.Map<AutorPatchDTO>(autorDB);
            // Que pueda tener validaciones

            patchDoc.ApplyTo(autorPatchDto, ModelState);
            var isValid = TryValidateModel(autorPatchDto);
            if (!isValid) { return ValidationProblem(); }

            _mapper.Map(autorPatchDto, autorDB);
            await context.SaveChangesAsync();

            return NoContent();

        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Deleted (int id)
        {
            var registrosBorrado = await context.Autores.Where(autor => autor.Id == id).ExecuteDeleteAsync();

            if (registrosBorrado == 0)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
