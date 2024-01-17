using Core.Contracts;
using Core.Entityes;
using Infrastructure.Context;

namespace Infrastructure.Repositories
{
    public class ComentarioRepository : BaseRepository<Comentario>, IComentarioRepository
    {
        public ComentarioRepository(NotiGestDbContext notiGestDbContext) : base(notiGestDbContext)
        {
        }
    }
}
