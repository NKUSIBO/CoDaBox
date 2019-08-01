using Microsoft.EntityFrameworkCore;

namespace Inocrea.CodaBox.ApiServer.Entities
{
    public class InosysDBContext : DbContext
    {
        public InosysDBContext() { }

        public InosysDBContext(DbContextOptions<InosysDBContext> options) : base(options) { }

        public DbSet<CompteBancaire> CompteBancaire { get; set; }
        //public DbSet<People> People { get; set; }
        public DbSet<Statements> Statements { get; set; }
        public DbSet<Transactions> Transactions { get; set; }

        // Unable to generate entity type for table 'history.HistoryPeople'. Please see the warning messages.
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=tcp:inocrea.database.windows.net,1433;Database=InosysDB;Persist Security Info=False;User Id=Inosys@inocrea;Password=Inocrea01!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;;MultipleActiveResultSets=true");
            }
        }
    }
}
