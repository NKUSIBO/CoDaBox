using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

namespace Inocrea.CodaBox.ApiServer.Entities
{
    public partial class InosysDBContext : DbContext
    {
        public InosysDBContext()
        {
        }

        public InosysDBContext(DbContextOptions<InosysDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<CompteBancaire> CompteBancaire { get; set; }
        public virtual DbSet<People> People { get; set; }
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<CompteBancaire>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("CompteBancaire_pk")
                    .ForSqlServerIsClustered(false);

                entity.Property(e => e.Bic)
                    .HasMaxLength(255)
                    .IsUnicode(false);

            

                entity.Property(e => e.CurrencyCode)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Iban)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.IdentificationNumber)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<People>(entity =>
            {
                entity.HasKey(e => e.PeopleId)
                    .HasName("People_pk")
                    .ForSqlServerIsClustered(false);

                entity.ToTable("People", "history");

                entity.Property(e => e.PeopleId).HasColumnName("PeopleID");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Statements>(entity =>
            {
                entity.HasKey(e => e.StatementId)
                    .HasName("Statements_pk")
                    .ForSqlServerIsClustered(false);

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.InformationalMessage)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.CompteBancaire)
                    .WithMany(p => p.Statements)
                    .HasForeignKey(d => d.CompteBancaireId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Statements_CompteBanquare_fk");
            });

            modelBuilder.Entity<Transactions>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("Transactions_pk")
                    .ForSqlServerIsClustered(false);

                entity.Property(e => e.Message)
                    .HasMaxLength(255)
                    .IsUnicode(false);

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
                    .HasConstraintName("Transactions_CompteBancaires_fk");

                entity.HasOne(d => d.Statement)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.StatementId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Transactions_Statements_fk");
            });
        }
    }
}
