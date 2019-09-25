using System.IO;
using Inocrea.CodaBox.ApiModel;
using Inocrea.CodaBox.ApiModel.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Inocrea.CodaBox.Back.Entities
{
    public class InosysDBContext : DbContext
    {
        private const string connectionString = "Server=tcp:inocrea.database.windows.net,1433;Initial Catalog=DbInosales;Persist Security Info=False;User ID=Inosys@inocrea;Password=Inocrea01!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        public InosysDBContext() { }

        public InosysDBContext(DbContextOptions<InosysDBContext> options) : base(options) { }

        public virtual DbSet<CompteBancaire> CompteBancaire { get; set; }
        public virtual DbSet<Statements> Statements { get; set; }
        public virtual DbSet<Transactions> Transactions { get; set; }
        public virtual DbSet<CodaIdentities> CodaIdentities { get; set; }

        public virtual DbSet<Company> Company { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");


            modelBuilder.Entity<CodaIdentities>(entity =>
            {
                entity.HasKey(e => e.CodaIdentityId)
                    .HasName("CodaIdentity_pk")
                    .ForSqlServerIsClustered(false);

                entity.Property(e => e.CodaIdentityId).HasColumnName("CodaIdentityID");

                entity.Property(e => e.Login)
                    .IsRequired()
                    .HasColumnName("login")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Pwd)
                    .IsRequired()
                    .HasColumnName("pwd")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.XCompany)
                    .IsRequired()
                    .HasColumnName("xCompany")
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });



            modelBuilder.Entity<Statements>(entity =>
            {
                entity.HasKey(e => e.StatementId)
                    .ForSqlServerIsClustered(false);

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.InformationalMessage)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.CompteBancaire)
                    .WithMany(p => p.Statements)
                    .HasForeignKey(d => d.CompteBancaireId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Statements_CompteBancaire");
            });

            modelBuilder.Entity<Transactions>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .ForSqlServerIsClustered(false);

                entity.Property(e => e.Message).HasColumnType("text");

                entity.Property(e => e.StatementId).HasColumnName("StatementID");

                entity.Property(e => e.StructuredMessage)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.TransactionDate).HasColumnType("datetime");

                entity.Property(e => e.ValueDate).HasColumnType("datetime");

                entity.HasOne(d => d.CompteBancaire)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.CompteBancaireId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Transactions_CompteBancaire");

                entity.HasOne(d => d.Statement)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.StatementId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Transactions_Statements_fk");
            });
        }
    }
}
