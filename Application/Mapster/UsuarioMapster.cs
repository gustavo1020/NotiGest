using Application.DTO;
using Core.Entityes;
using Mapster;

namespace Application.Mapster
{
    public class UsuarioMapster : IRegister
    {
        public void Register(TypeAdapterConfig type)
        {
            type.NewConfig<User, UsuarioDto>();
            type.NewConfig<User, UserInsertDto>();
        }
    }
}
