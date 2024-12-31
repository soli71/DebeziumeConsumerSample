using Microsoft.EntityFrameworkCore;

namespace BCSample.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {

        }

        public DbSet<Outbox> Outbox { get; set; }
    }
}
