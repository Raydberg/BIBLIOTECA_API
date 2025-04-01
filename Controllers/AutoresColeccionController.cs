using AutoMapper;
using BIBLIOTECA_API.DB;
using BIBLIOTECA_API.DTOs;
using BIBLIOTECA_API.Entidades;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BIBLIOTECA_API.Controllers
{
    [Route("api/autores-coleccion")]
    [ApiController]
    public class AutoresColeccionController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public AutoresColeccionController(ApplicationDbContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        // GET: api/<AutoresColeccionController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<AutoresColeccionController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<AutoresColeccionController>
        [HttpPost]
        public async Task<ActionResult> Post(IEnumerable<AutorCreateResponseDTO> autorCreateDto)
        {
            var autors = _mapper.Map<IEnumerable<Autor>>(autorCreateDto);
            // Agregar un Listado de entidades
            _context.AddRange(autors);
            await _context.SaveChangesAsync();
            return Ok() ;
        }

        // PUT api/<AutoresColeccionController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<AutoresColeccionController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
