using Core.Contracts;
using Core.Entityes;
using Infrastructure.Context;

namespace Infrastructure.Repositories
{
    public class NoticiaRepository : BaseRepository<Noticia>, INoticiaRepository
    {
        public NoticiaRepository(NotiGestDbContext notiGestDbContext) : base(notiGestDbContext)
        {
        }
    }
}
