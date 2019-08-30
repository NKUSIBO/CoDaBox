using System.IO;
using Inocrea.CodaBox.ApiModel.Models;
using Inocrea.CodaBox.ApiModel.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Inocrea.CodaBox.ApiServer.Entities
{
    public class InosysDBContext : DbContext
    {

        public InosysDBContext() { }

        public InosysDBContext(DbContextOptions<InosysDBContext> options) : base(options) { }

    
        public virtual DbSet<CompteBancaire> CompteBancaire { get; set; }
        public virtual DbSet<Statements> Statements { get; set; }
        public virtual DbSet<Transactions> Transactions { get; set; }
        public virtual DbSet<CodaIdentity> CodaIdentities { get; set; }

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
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            //modelBuilder.Ignore<IdentityUserRole<string>>();

            modelBuilder.Entity<Company>(entity =>
            {
                entity.Property(e => e.Tva).HasMaxLength(50);
            });
           

            modelBuilder.Entity<CompteBancaire>(entity =>
            {
                entity.HasIndex(e => e.CompanyId);

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.CompteBancaire)
                    .HasForeignKey(d => d.CompanyId);
            });

          
            modelBuilder.Entity<Statements>(entity =>
            {
                entity.HasKey(e => e.StatementId)
                    .ForSqlServerIsClustered(false);

                entity.HasIndex(e => e.CompteBancaireId);

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

                entity.HasIndex(e => e.CompteBancaireId);

                entity.HasIndex(e => e.StatementId);

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