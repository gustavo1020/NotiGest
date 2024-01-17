using Application.DTO;
using Application.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Application.Contracts
{
    public interface IComentarioService
    {
        Task<IActionResult> Crear(ComentarioInsertDto comentarioInsertDto);
        Task<IActionResult> Eliminar(Guid id);
        Task<IActionResult> Actualizar(Guid id, ComentarioUpdateDto comentarioUpdateDto);
        Task<IActionResult> ObtenerId(Guid id, Filter predicateComentario);
    }

}
