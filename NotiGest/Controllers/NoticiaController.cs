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
    public class NoticiaController : ControllerBase
    {
        public readonly INoticiaService _NoticiaService;
        public NoticiaController(INoticiaService noticiaService)
        {
            this._NoticiaService = noticiaService;
        }

        [HttpGet]
        public async Task<ActionResult<Paginated<NoticiaDto>>> GetAll(int cantItemForPage = 10, int pageNumber = 0, bool nuevos = false, bool prioridad = false)
        {
            try
            {
                Filter predicateNoticia = new Filter() { cantItemForPage = cantItemForPage, pageNumber = pageNumber, nuevos = nuevos, prioridad = prioridad };
                var noticias = await _NoticiaService.ObtenerTodo(predicateNoticia);

                return Ok(noticias);
            }
            catch (Exception ex)
            {
                return StatusCode( 500, ex.Message);
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] NoticiaInsertDto noticiaInsertDto)
        {
            try
            {
                return Ok(await _NoticiaService.Crear(noticiaInsertDto));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
                throw;
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] NoticiaUpdateDto noticiaUpdateDto)
        {
            try
            {
                return Ok(await _NoticiaService.Actualizar(id, noticiaUpdateDto));
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
                return Ok(await _NoticiaService.Eliminar(id));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
                throw;
            }
        }
    }
}
