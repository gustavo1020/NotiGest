using Application.Contracts;
using Application.DTO;
using Application.Page;
using Application.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace NotiGest.Controllers
{
    [Produces("application/json")]
    [ApiController]
    [Route("api/v1/[controller]/[action]")]
    [Authorize(Roles = "Administrador, UserDefault")]
    public class ComentarioController : ControllerBase
    {
        public readonly IComentarioService _ComentarioService;
        public ComentarioController(IComentarioService comentarioService)
        {
            this._ComentarioService = comentarioService;
        }
        [HttpGet]
        public async Task<ActionResult<Paginated<ComentarioDto>>> GetAll(Guid id, int cantItemForPage = 10, int pageNumber = 0, bool nuevos = false, bool prioridad = false)
        {
            try
            {
                Filter predicateComentario = new Filter() { cantItemForPage = cantItemForPage, pageNumber = pageNumber, nuevos = nuevos, prioridad = prioridad };

                var comentarios = await _ComentarioService.ObtenerId(id, predicateComentario);
                return Ok(comentarios);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ComentarioInsertDto comentarioInsertDto)
        {
            try
            {
                return Ok(await _ComentarioService.Crear(comentarioInsertDto));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
                throw;
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] ComentarioUpdateDto comentarioUpdateDto)
        {
            try
            {
                return Ok(await _ComentarioService.Actualizar(id, comentarioUpdateDto));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
                throw;
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                return Ok(await _ComentarioService.Eliminar(id));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
                throw;
            }
        }
    }
}
