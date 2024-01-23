using Application.DTO;
using Application.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Application.Contracts
{
    public interface INoticiaService
    {

        Task<IActionResult> Crear(NoticiaInsertDto noticiaInsertDto);
        Task<IActionResult> Eliminar(Guid id);
        Task<IActionResult> Actualizar(Guid id, NoticiaUpdateDto noticiaUpdateDto);
        Task<IActionResult> ObtenerTodo(Filter predicateNoticia, string text);

    }
}
