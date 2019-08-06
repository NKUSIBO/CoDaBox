using System.IO;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Inocrea.CodaBox.ApiServer.Entities
{
    public class InosysDBContext : DbContext
    {
        public InosysDBContext() {}

        public InosysDBContext(DbContextOptions<InosysDBContext> options) : base(options){}

        public virtual DbSet<CompteBancaire> CompteBancaire { get; set; }
        public virtual DbSet<Statements> Statements { get; set; }
        public virtual DbSet<Transactions> Transactions { get; set; }

        // Unable to generate entity type for table 'history.HistoryPeople'. Please see the warning messages.

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();
                var connectionString = configuration.GetConnectionString("DefaultConnection");
                optionsBuilder.UseSqlServer(connectionString);
            }
        }
    }
}
