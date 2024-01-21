using Application.Contracts;
using Application.DTO;
using Core.Entityes;
using Mapster;
using Microsoft.AspNetCore.Identity;
using RestSharp;

namespace Application.BackgroundService
{
    public interface ITaskNoticiaService
    {
        Task GenerarNoticias();
    }

    public class TaskNoticiaService : ITaskNoticiaService
    {
        private readonly RestClient _restClient;
        private readonly INoticiaService _noticiaService;
        private readonly UserManager<User> _userManager;

        public TaskNoticiaService(RestClient restClient, INoticiaService _noticiaService, UserManager<User> userManager)
        {
            this._restClient = restClient;
            this._noticiaService = _noticiaService;
            this._userManager = userManager;
        }

        public async Task GenerarNoticias()
        {
            var noticias = await _restClient.GetJsonAsync<Root[]>("https://eodhd.com/api/news?s=AAPL.US&offset=0&limit=100&api_token=demo&fmt=json");
            var user = await _userManager.FindByEmailAsync("Gustavo@admin.com");
            foreach (var noticia in noticias)
            {
                var noticiasDto = noticia?.Adapt<NoticiaInsertDto>();
                if (noticiasDto != null && user != null)
                {
                    noticiasDto.Usuario = user.Id;
                    noticiasDto.EnlaceImagen = "https://www.oas.org/ext/Portals/14/EasyDNNnews/1892/Image.jpg";

                    await _noticiaService.Crear(noticiasDto);
                }
            }
        }
    }
}
