using Application.Contracts;
using Application.DTO;
using Application.Page;
using Application.Utils;
using Core.Contracts;
using Core.Entityes;
using FluentValidation;
using FluentValidation.Results;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class ComentarioService : IComentarioService
    {
        private readonly IComentarioRepository _Comentariorepository;
        private readonly IValidator<Comentario> _ComentarioValidator;
        public ComentarioService(IComentarioRepository Comentariorepository, IValidator<Comentario> comentarioValidator) 
        {
            this._Comentariorepository = Comentariorepository;
            this._ComentarioValidator = comentarioValidator;
        }
        public async Task<IActionResult> Crear(ComentarioInsertDto comentarioInsertDto )
        {
            try
            {
                var entity = comentarioInsertDto.Adapt<Comentario>();
                entity.FechaBaja = null;

                ValidationResult result = await _ComentarioValidator.ValidateAsync(entity);

                if (!result.IsValid)
                {
                    return new BadRequestObjectResult(new { Mensaje = "Error de validación", Errores = result.Errors, Exitoso = false });
                }
                
                var state = await _Comentariorepository.AddEntityAsync(entity);

                return new OkObjectResult(new { Mensaje = "Comentario creado exitosamente", Exitoso = state });
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(new { Mensaje = "Error al crear el comentario", Errores = ex.Message, Exitoso = false });
            }
        }
        public async Task<IActionResult> Eliminar(Guid id)
        {
            try
            {
                var comentario = await _Comentariorepository.GetByIdAsync(id);

                if (comentario == null) return new BadRequestObjectResult(new { Mensaje = "Error al buscar el comentario", Errores = comentario, Exitoso = false });

                comentario.FechaBaja = DateTime.Now;

                var state = await _Comentariorepository.DeleteEntityAsync(comentario);

                return new OkObjectResult(new { Mensaje = "Comentario eliminado exitosamente", Exitoso = state });
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(new { Mensaje = "Error al eliminar el comentario", Errores = ex.Message, Exitoso = false });
            }
        }
        public async Task<IActionResult> Actualizar(Guid id, ComentarioUpdateDto comentarioUpdateDto )
        {
            try
            {
                var comentario = await _Comentariorepository.GetByIdAsync(id);

                if (comentario == null) return new BadRequestObjectResult(new { Mensaje = "Error al buscar el comentario", Errores = comentario, Exitoso = false });

                var entity = comentarioUpdateDto.Adapt(comentario);

                ValidationResult result = await _ComentarioValidator.ValidateAsync(entity);

                if (!result.IsValid)
                {
                    return new BadRequestObjectResult(new { Mensaje = "Error de validación", Errores = result.Errors, Exitoso = false });
                }

                var state = await _Comentariorepository.UpdateEntityAsync(entity);

                return new OkObjectResult(new { Mensaje = "Comentario actualizado exitosamente", Exitoso = state });
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(new { Mensaje = "Error al actualizar el comentario", Errores = ex.Message, Exitoso = false });
            }
        }
        public async Task<IActionResult> ObtenerId(Guid id, Filter predicateComentario)
        {
            try
            {
                var comentarios = await _Comentariorepository.GetAllEntityAsync(x => x.NoticiaId == id && x.FechaBaja == null, a => a.Include(n => n.Autor));

                var comentariosFilters = comentarios.Skip(predicateComentario.cantItemForPage * predicateComentario.pageNumber).Take(predicateComentario.cantItemForPage).ToList();

                if (predicateComentario.nuevos) comentariosFilters = comentariosFilters.OrderByDescending(x => x.CreatedDate).ToList();

                if (predicateComentario.prioridad) comentariosFilters = comentariosFilters.OrderByDescending(x => x.Destacado == true).ToList();

                var comentariosDto = comentariosFilters.Adapt<IEnumerable<ComentarioDto>>();

                Paginated<ComentarioDto> response = new Paginated<ComentarioDto>()
                {
                    Resultado = comentariosDto,
                    CantItemForPage = predicateComentario.cantItemForPage,
                    PageNumber = predicateComentario.pageNumber,
                    AllItems = comentarios.Count()
                };

                return new OkObjectResult(response);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(new { Mensaje = "Error al obtener los comentarios", Errores = ex.Message });
            }
        }

    }
}
