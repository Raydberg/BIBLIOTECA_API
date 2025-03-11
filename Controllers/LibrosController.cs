using BIBLIOTECA_API.DB;
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

        public LibrosController (ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<IEnumerable<Libro>> Get ()
        {
            return await context.Libros.ToListAsync();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Libro>> Get (int id)
        {
            var libro = await context.Libros
                // Esto lo que hace es incluir al autor en el resultado
                // Ya que tenemos la propieda de navegacion
                .Include(libro => libro.Autor)
                .FirstOrDefaultAsync(libro => libro.Id == id);
            if (libro is Nullable)
            {
                return NotFound("Libro no encontrado");
            }

            return libro;
        }

        [HttpPost]
        public async Task<ActionResult> Post (Libro libro)
        {
            // Permite obtener true o false , que cuumpla la condicion dada
            var isAutor = await context.Autores.AnyAsync(autor => autor.Id == libro.AutorId);
            if (!isAutor)
            {
                return BadRequest($"El autor de id {libro.AutorId} no existe");
            }
            context.Add(libro);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put (int id, Libro libro)
        {
            if (id != libro.Id)
            {
                return BadRequest();
            }
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
