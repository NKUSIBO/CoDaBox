using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Inocrea.CodaBox.Web.Entities
{
    public partial class InosysDB_TestContext : DbContext
    {
        public InosysDB_TestContext()
        {
        }

        public InosysDB_TestContext(DbContextOptions<InosysDB_TestContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AspNetRoleClaims> AspNetRoleClaims { get; set; }
        public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUserRoles> AspNetUserRoles { get; set; }
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        public virtual DbSet<AspNetUserTokens> AspNetUserTokens { get; set; }
        public virtual DbSet<CodaIdentities> CodaIdentities { get; set; }
        public virtual DbSet<Company> Company { get; set; }
        public virtual DbSet<CompteBancaire> CompteBancaire { get; set; }
        public virtual DbSet<People> People { get; set; }
        public virtual DbSet<SepaDirectDebits> SepaDirectDebits { get; set; }
        public virtual DbSet<Statements> Statements { get; set; }
        public virtual DbSet<Transactions> Transactions { get; set; }

        // Unable to generate entity type for table 'history.HistoryPeople'. Please see the warning messages.

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=tcp:inocrea.database.windows.net,1433;Database=InosysDB_Test;Persist Security Info=False;User ID=Inosys@inocrea;Password=Inocrea01!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;;MultipleActiveResultSets=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AspNetRoleClaims>(entity =>
            {
                entity.HasIndex(e => e.RoleId);

                entity.Property(e => e.RoleId).IsRequired();

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetRoleClaims)
                    .HasForeignKey(d => d.RoleId);
            });

            modelBuilder.Entity<AspNetRoles>(entity =>
            {
                entity.HasIndex(e => e.NormalizedName)
                    .HasName("RoleNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedName] IS NOT NULL)");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.NormalizedName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetUserClaims>(entity =>
            {
                entity.HasIndex(e => e.UserId);

                entity.Property(e => e.UserId).IsRequired();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserClaims)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserLogins>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

                entity.HasIndex(e => e.UserId);

                entity.Property(e => e.UserId).IsRequired();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserLogins)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserRoles>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });

                entity.HasIndex(e => e.RoleId);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.RoleId);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUsers>(entity =>
            {
                entity.HasIndex(e => e.NormalizedEmail)
                    .HasName("EmailIndex");

                entity.HasIndex(e => e.NormalizedUserName)
                    .HasName("UserNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedUserName] IS NOT NULL)");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.FirstName).HasMaxLength(200);

                entity.Property(e => e.IdCompany).HasMaxLength(50);

                entity.Property(e => e.LastName).HasMaxLength(250);

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

                entity.Property(e => e.UserName).HasMaxLength(256);

                entity.HasOne(d => d.IdCompanyNavigation)
                    .WithMany(p => p.AspNetUsers)
                    .HasForeignKey(d => d.IdCompany)
                    .HasConstraintName("FK_AspNetUsers_Company");
            });

            modelBuilder.Entity<AspNetUserTokens>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserTokens)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<CodaIdentities>(entity =>
            {
                entity.HasKey(e => e.CodaIdentityId)
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

            modelBuilder.Entity<Company>(entity =>
            {
                entity.HasKey(e => e.Tva);

                entity.Property(e => e.Tva)
                    .HasColumnName("TVA")
                    .HasMaxLength(50)
                    .ValueGeneratedNever();

                entity.Property(e => e.ActivitSic)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.CompanyName).IsRequired();

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.Nentreprise)
                    .IsRequired()
                    .HasColumnName("NEntreprise");

                entity.Property(e => e.NumeroClient)
                    .IsRequired()
                    .HasMaxLength(10);
            });

            modelBuilder.Entity<CompteBancaire>(entity =>
            {
                entity.HasKey(e => e.Id)
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

                entity.Property(e => e.OwnerTva).HasMaxLength(50);

                entity.HasOne(d => d.OwnerTvaNavigation)
                    .WithMany(p => p.CompteBancaire)
                    .HasForeignKey(d => d.OwnerTva)
                    .HasConstraintName("FK_CompteBancaire_Company");
            });

            modelBuilder.Entity<People>(entity =>
            {
                entity.HasKey(e => e.PeopleId)
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

            modelBuilder.Entity<SepaDirectDebits>(entity =>
            {
                entity.HasKey(e => e.SepaDirectDebitId)
                    .ForSqlServerIsClustered(false);

                entity.Property(e => e.CreditorIdentificationCode)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.MandateReference)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.Transaction)
                    .WithMany(p => p.SepaDirectDebits)
                    .HasForeignKey(d => d.TransactionId)
                    .HasConstraintName("SepaDirectDebits_Transactions_fk");
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
                    .HasConstraintName("Statements_CompteBanquare_fk");
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
