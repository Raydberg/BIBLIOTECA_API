using AutoMapper;
using BIBLIOTECA_API.DB;
using BIBLIOTECA_API.DTOs;
using BIBLIOTECA_API.Entidades;
using BIBLIOTECA_API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BIBLIOTECA_API.Controllers
{
    [Route("api/libros/{libroId:int}/commentarios")]
    [ApiController]
    [Authorize]
    public class CommentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IServiceUsers _serviceUsers;

        public CommentsController(ApplicationDbContext context, IMapper mapper, IServiceUsers serviceUsers)
        {
            _context = context;
            _mapper = mapper;
            _serviceUsers = serviceUsers;
        }


        [HttpGet]
        public async Task<ActionResult<List<CommentDTO>>> Get(int libroId)
        {
            var isLibro = await _context.Libros.AnyAsync(libro => libro.Id == libroId);

            if (!isLibro)
            {
                return NotFound();
            }

            var comments = await _context.Comments
                .Include(x => x.Usuario)
                .Where(comment => comment.LibroId == libroId)
                .OrderByDescending(comment => comment.FechaPublicacion)
                .ToListAsync();

            return _mapper.Map<List<CommentDTO>>(comments);
        }

        [HttpGet("{id}", Name = "ObtenerComentario")]
        public async Task<ActionResult<CommentDTO>> Get(Guid id)
        {
            var comment = await _context.Comments
                .Include(x => x.Usuario)
                .FirstOrDefaultAsync(comment => comment.Id == id);


            if (comment == null)
            {
                return NotFound();
            }

            return _mapper.Map<CommentDTO>(comment);
        }


        [HttpPost]
        public async Task<ActionResult<Comment>> Post(int libroId, CommentCreateDTO commentCreateDto)
        {
            var isLibro = await _context.Libros.AnyAsync(libro => libro.Id == libroId);

            if (!isLibro)
            {
                return NotFound();
            }

            var usuario = await _serviceUsers.GetUser();
            if (usuario is null)
            {
                return NotFound();
            }

            var comment = _mapper.Map<Comment>(commentCreateDto);
            comment.LibroId = libroId;
            comment.FechaPublicacion = DateTime.UtcNow;
            comment.UserId = usuario.Id;


            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            var commentDto = _mapper.Map<CommentCreateDTO>(comment);

            return CreatedAtRoute("ObtenerComentario", new { id = comment.Id, libroId }, commentDto);
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult> Patch(Guid id, int libroId, JsonPatchDocument<CommentPatchDTO> patchDoc)
        {
            if (patchDoc is null)
            {
                return BadRequest();
            }

            var isLibro = await _context.Libros.AnyAsync(libro => libro.Id == libroId);

            if (!isLibro)
            {
                return NotFound();
            }
            var usuario = await _serviceUsers.GetUser();
            if (usuario is null)
            {
                return NotFound();
            }
            var commentDB = await _context.Comments.FirstOrDefaultAsync(comment => comment.Id == id);
            if (commentDB is null)
            {
                return NotFound();
            }

            if (commentDB.UserId != usuario.Id)
            {
                // Prohibir el acceso
                return Forbid();
            }
            // Mapeo
            var commentPatchDto = _mapper.Map<CommentPatchDTO>(commentDB);
            // Que pueda tener validaciones

            patchDoc.ApplyTo(commentPatchDto, ModelState);

            var isValid = TryValidateModel(commentPatchDto);
            if (!isValid)
            {
                return ValidationProblem();
            }

            _mapper.Map(commentPatchDto, commentDB);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id, int libroId)
        {
            var isLibro = await _context.Libros.AnyAsync(libro => libro.Id == libroId);

            if (!isLibro)
            {
                return NotFound();
            }

            var user = await _serviceUsers.GetUser();
            if (user is null)
            {
                return NotFound();
            }
            var commentDB = await _context.Comments.FirstOrDefaultAsync(x => x.Id == id)!;
            if (commentDB is null)
            {
                return NotFound();
            }

            if (commentDB.UserId != user.Id)
            {
                return Forbid();
            }
            
            //Otra manera de borrar registros
            _context.Remove(commentDB); //-> Solo lo marca para hacer borrado hasta que hagamos un saveChanges
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}