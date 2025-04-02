using AutoMapper;
using BIBLIOTECA_API.DB;
using BIBLIOTECA_API.DTOs;
using BIBLIOTECA_API.Entidades;
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

        public CommentsController (ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<ActionResult<List<CommentDTO>>> Get (int libroId)
        {
            var isLibro = await _context.Libros.AnyAsync(libro => libro.Id == libroId);

            if (!isLibro) { return NotFound(); }

            var comments = await _context.Comments.Where(comment => comment.LibroId == libroId)
                .OrderByDescending(comment => comment.FechaPublicacion).ToListAsync();

            return _mapper.Map<List<CommentDTO>>(comments);
        }

        [HttpGet("{id}", Name = "ObtenerComentario")]
        public async Task<ActionResult<CommentDTO>> Get (Guid id)
        {
            var comment = await _context.Comments.FirstOrDefaultAsync(comment => comment.Id == id);


            if (comment == null)
            {
                return NotFound();
            }

            return _mapper.Map<CommentDTO>(comment);
        }



        [HttpPost]
        public async Task<ActionResult<Comment>> Post (int libroId, CommentCreateDTO commentCreateDto)
        {

            var isLibro = await _context.Libros.AnyAsync(libro => libro.Id == libroId);

            if (!isLibro) { return NotFound(); }

            var comment = _mapper.Map<Comment>(commentCreateDto);
            comment.LibroId = libroId;
            comment.FechaPublicacion = DateTime.UtcNow;



            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            var commentDto = _mapper.Map<CommentCreateDTO>(comment);

            return CreatedAtRoute("ObtenerComentario", new { id = comment.Id, libroId }, commentDto);
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult> Patch (Guid id, int libroId, JsonPatchDocument<CommentPatchDTO> patchDoc)
        {
            if (patchDoc is null) { return BadRequest(); }
            var isLibro = await _context.Libros.AnyAsync(libro => libro.Id == libroId);

            if (!isLibro) { return NotFound(); }

            var commentDB = await _context.Comments.FirstOrDefaultAsync(comment => comment.Id == id);
            if (commentDB is null) { return NotFound(); }

            // Mapeo
            var commentPatchDto = _mapper.Map<CommentPatchDTO>(commentDB);
            // Que pueda tener validaciones

            patchDoc.ApplyTo(commentPatchDto, ModelState);

            var isValid = TryValidateModel(commentPatchDto);
            if (!isValid) { return ValidationProblem(); }

            _mapper.Map(commentPatchDto, commentDB);
            await _context.SaveChangesAsync();

            return NoContent();

        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete (Guid id, int libroId)
        {

            var isLibro = await _context.Libros.AnyAsync(libro => libro.Id == libroId);

            if (!isLibro) { return NotFound(); }


            var commentDeleted = await _context.Comments.Where(comment => comment.Id == id).ExecuteDeleteAsync();


            if (commentDeleted == 0)
            {
                return NotFound();
            }

            //_context.Comments.Remove(comment);
            //await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
