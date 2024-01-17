using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Context
{
    public class HangfireContext : DbContext
    {
        public HangfireContext()
        {
        }

        public HangfireContext(DbContextOptions<HangfireContext> options) : base(options)
        {

        }
    }
}
