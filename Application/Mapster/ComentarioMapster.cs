using Application.DTO;
using Core.Entityes;
using Mapster;

namespace Application.Mapster
{
    public class ComentarioMapster : IRegister
    {
        public void Register (TypeAdapterConfig type) 
        {
            type.NewConfig<Comentario, ComentarioDto>();
            type.NewConfig<Comentario, ComentarioUpdateDto>();
            type.NewConfig<Comentario, ComentarioInsertDto>();
        }
    }
}
