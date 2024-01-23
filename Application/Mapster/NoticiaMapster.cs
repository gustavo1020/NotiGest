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
            type.NewConfig<Root, NoticiaInsertDto>().Map(dest => dest.Titulo, src => src.title).Map(dest => dest.Contenido, src => src.content);
        }
    }
}
