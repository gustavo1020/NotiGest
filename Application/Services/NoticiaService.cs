using Mapster;
using Microsoft.EntityFrameworkCore;
using FluentValidation.Results;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Core.Contracts;
using Core.Entityes;
using Application.Contracts;
using Application.DTO;
using Application.Page;
using Application.Utils;
using System.Linq;

namespace NotiGest.Services
{
    public class NoticiaService : INoticiaService
    {
        private readonly INoticiaRepository _NoticiaRepository;
        private readonly IComentarioRepository _ComentarioRepository;
        private readonly IValidator<Noticia> _NoticiaValidator;
        private readonly IRedisService<Noticia> _redisService;
        private readonly string key = "Comentarios";
        public NoticiaService(INoticiaRepository NoticiaRepository, IComentarioRepository ComentarioRepository, IValidator<Noticia> noticiaValidator, IRedisService<Noticia> redisService)
        {
            this._NoticiaRepository = NoticiaRepository;
            this._ComentarioRepository = ComentarioRepository;
            this._NoticiaValidator = noticiaValidator;
            this._redisService = redisService;
        }

        public async Task<IActionResult> Crear(NoticiaInsertDto noticiaInsertDto)
        {
            try
            {
                var entity = noticiaInsertDto.Adapt<Noticia>();
                entity.FechaBaja = null;
                ValidationResult result = await _NoticiaValidator.ValidateAsync(entity);
                if (!result.IsValid)
                {
                    return new BadRequestObjectResult(new { Mensaje = "Error de validación", Errores = result.Errors, Exitoso = false });
                }

                var state = await _NoticiaRepository.AddEntityAsync(entity);

#pragma warning disable CS4014

                // No esperamos a que la tarea en paralelo se complete
                await Task.Run(async () => await _redisService.SaveCache(key, await GetNoticias()));

#pragma warning restore CS4014

                return new OkObjectResult(new { Mensaje = "Noticia creada exitosamente", Exitoso = state });
            }
            catch (DbUpdateException ex)
            {
                var innerException = ex.InnerException;
                return new BadRequestObjectResult(new { Mensaje = "Error al crear la noticia", Errores = ex.Message, Exitoso = false });
            }

        }
        public async Task<IActionResult> Eliminar(Guid id)
        {
            try
            {
                var noticia = await _NoticiaRepository.GetByIdAsync(id);

                if (noticia == null) return new BadRequestObjectResult(new { Mensaje = "Error al buscar la noticia", Errores = noticia, Exitoso = false });

                noticia.FechaBaja = DateTime.Now;

                var state = await _NoticiaRepository.DeleteEntityAsync(noticia);

                var comenatariosAsociados = await _ComentarioRepository.GetAllEntityAsync(x => x.NoticiaId == id);

                foreach (var comenatarioAsociado in comenatariosAsociados)
                {
                    comenatarioAsociado.FechaBaja = DateTime.Now;
                    await _ComentarioRepository.DeleteEntityAsync(comenatarioAsociado);
                }

#pragma warning disable CS4014

                // No esperamos a que la tarea en paralelo se complete
                await Task.Run(async () => await _redisService.SaveCache(key, await GetNoticias()));

#pragma warning restore CS4014

                return new OkObjectResult(new { Mensaje = "Noticia eliminada exitosamente", Exitoso = state });
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(new { Mensaje = "Error al eliminar la noticia", Errores = ex.Message, Exitoso = false });
            }
        }
        public async Task<IActionResult> Actualizar(Guid id, NoticiaUpdateDto noticiaUpdateDto)
        {
            try
            {
                var noticia = await _NoticiaRepository.GetByIdAsync(id);

                if (noticia == null) return new BadRequestObjectResult(new { Mensaje = "Error al buscar la noticia", Errores = noticia, Exitoso = false });

                var entity = noticiaUpdateDto.Adapt(noticia);

                ValidationResult result = await _NoticiaValidator.ValidateAsync(entity);
                if (!result.IsValid)
                {
                    return new BadRequestObjectResult(new { Mensaje = "Error de validación", Errores = result.Errors, Exitoso = false });
                }

                var state = await _NoticiaRepository.UpdateEntityAsync(entity);

#pragma warning disable CS4014

                // No esperamos a que la tarea en paralelo se complete
                await Task.Run(async () => await _redisService.SaveCache(key, await GetNoticias()));

#pragma warning restore CS4014

                return new OkObjectResult(new { Mensaje = "Noticia actualizada exitosamente", Exitoso = state });
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(new { Mensaje = "Error al actualizar la noticia", Errores = ex.Message, Exitoso = false });
            }

        }
        public async Task<IActionResult> ObtenerTodo(Filter predicateNoticia, string text)
        {
            try
            {
                var palabras = text.ToLower().Split(' ');
                var queryableNoticias = await _redisService.SerchCache(key);

                if (queryableNoticias.Count() == 0)
                {
                    queryableNoticias = await GetNoticias();
                }

                if (text.Trim() != string.Empty) queryableNoticias = queryableNoticias.Where(x => palabras.Any(palabra => x.Titulo.ToLower().Contains(palabra)));

                if (predicateNoticia.nuevos) queryableNoticias = queryableNoticias.OrderByDescending(x => x.CreatedDate).ToList();

                if (predicateNoticia.prioridad) queryableNoticias = queryableNoticias.OrderByDescending(x => x.Destacado == true).ToList();

                var noticiasFilters = queryableNoticias.Skip(predicateNoticia.cantItemForPage * predicateNoticia.pageNumber).Take(predicateNoticia.cantItemForPage).ToList();

                var noticiasDto = noticiasFilters.Adapt<IEnumerable<NoticiaDto>>();

                Paginated<NoticiaDto> response = new Paginated<NoticiaDto>()
                {
                    Resultado = noticiasDto,
                    CantItemForPage = predicateNoticia.cantItemForPage,
                    PageNumber = predicateNoticia.pageNumber,
                    AllItems = queryableNoticias.Count()
                };

                return new OkObjectResult(response);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(new { Mensaje = "Error al obtener las noticias", Errores = ex.Message });
            }
        }

        public async Task<IEnumerable<Noticia>> GetNoticias()
        {
            var noticias = await _NoticiaRepository.GetAllEntityAsync(x => x.FechaBaja == null, a => a.Include(n => n.Autor));
            return noticias;
        }
    }
}
