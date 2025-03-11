using BIBLIOTECA_API.DB;
using BIBLIOTECA_API.Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BIBLIOTECA_API.Controllers
{
    [ApiController] //-> Se refiere a un controlador 
    [Route("api/autores")] //-> Ruta en la que se encuentra el controlador 
    public class AutoresController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        // Inyeccion de dependencia para nuestro ApplicationDbContext
        public AutoresController (ApplicationDbContext context)
        {
            this.context = context;
        }
        [HttpGet]
        public async Task<IEnumerable<Autor>> Get ()
        {
            // Usamos el context para luego ir a la tabla autores
            // Y traer la data de manera asincrona
            return await context.Autores.ToListAsync();
        }

        // Si vamos a usar dos metodos GET tenemos que diferenciarlos
        // En este caso vamos a llamar mediante el ID y tendria que ir asi como estan juntos

        /**
         * El ActionResult como su nombre lo indica es el resultado de una action
         * siempre que vayamos a enviar una data para esperar algo a cambio es
         * conveniente usarlo
         */
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Autor>> Get (int id)
        {
            var autor = await context.Autores.FirstOrDefaultAsync(autor => autor.Id == id);

            if (autor is null)
            {
                return NotFound();
            }

            return autor;
        }

        [HttpPost]
        public async Task<ActionResult> Post (Autor autor)
        {
            context.Add(autor);
            // Guardar cambios de manera asincrona
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put (int id, Autor autor)
        {
            if (id != autor.Id)
            {
                return BadRequest("Los ids deben conicidir");
            }

            context.Update(autor);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Deleted (int id)
        {
            var registrosBorrado = await context.Autores.Where(autor => autor.Id == id).ExecuteDeleteAsync();
            if (registrosBorrado == 0)
            {
                return NotFound();
            }

            return Ok();
        }
    }
}
