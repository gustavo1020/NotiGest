using Application.DTO;
using Core.Entityes;
using Mapster;

namespace Application.Mapster
{
    public class NoticiaMapster : IRegister
    {
        public void Register(TypeAdapterConfig type)
        {
            type.NewConfig<Noticia, NoticiaDto>();
            type.NewConfig<Noticia, NoticiaUpdateDto>();
            type.NewConfig<NoticiaInsertDto, Noticia>().Map(dest => dest.AutorId, src => src.Usuario);
            type.NewConfig<Root, NoticiaInsertDto>().Map(dest => dest.Titulo, src => src.title.Count() >= 100 ? src.title.Substring(0, 99) : src.title).Map(dest => dest.Contenido, src => src.content.Count() >= 500 ? src.content.Substring(0, 499): src.content);
        }
    }
}
