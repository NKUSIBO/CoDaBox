using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Inocrea.CodaBox.ApiServer.Models
{
    public partial class DevCodaBoxContext : DbContext
    {
        public DevCodaBoxContext()
        {
        }



        public DevCodaBoxContext(DbContextOptions<DevCodaBoxContext> options)
            : base(options)
        {
        }

        public virtual DbSet<CompteBancaire> CompteBancaire { get; set; }
        public virtual DbSet<Statements> Statements { get; set; }
        public virtual DbSet<Transactions> Transactions { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<CompteBancaire>(entity =>
            {
                entity.Property(e => e.Bic)
                    .IsRequired()
                    .HasColumnName("BIc")
                    .HasMaxLength(50);

                entity.Property(e => e.CurrencyCode).IsRequired();

                entity.Property(e => e.Iban)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.IdentificationNumber).HasMaxLength(50);
            });

            modelBuilder.Entity<Statements>(entity =>
            {
                entity.HasKey(e => e.StatementId);

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.InformationalMessage).IsRequired();

                entity.Property(e => e.InitialBalance).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.NewBalance).HasColumnType("decimal(18, 0)");

                entity.HasOne(d => d.CompteBancaire)
                    .WithMany(p => p.Statements)
                    .HasForeignKey(d => d.CompteBancaireId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Statements_Iban");
            });

            modelBuilder.Entity<Transactions>(entity =>
            {
                entity.Property(e => e.Amount).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.StructuredMessage).IsRequired();

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
                    .HasConstraintName("FK_Transactions_Statements");
            });
        }
    }
}
