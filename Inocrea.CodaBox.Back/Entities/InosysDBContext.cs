using Inocrea.CodaBox.ApiModel.Models;
using Microsoft.EntityFrameworkCore;

namespace Inocrea.CodaBox.Back.Entities
{
    public class InosysDBContext : DbContext
    {
        private const string connectionString = "Server=tcp:inocrea.database.windows.net,1433;Database=InosysDB;Persist Security Info=False;User ID=Inosys@inocrea;Password=Inocrea01!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;;MultipleActiveResultSets=true";

        public InosysDBContext() { }

        public InosysDBContext(DbContextOptions<InosysDBContext> options) : base(options) { }


        public virtual DbSet<CompteBancaire> CompteBancaire { get; set; }
        public virtual DbSet<Statements> Statements { get; set; }
        public virtual DbSet<Transactions> Transactions { get; set; }
        public virtual DbSet<CodaIdentity> CodaIdentities { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}