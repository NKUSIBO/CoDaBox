//using System;
//using Inocrea.CodaBox.ApiServer.Models;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata;

//namespace Inocrea.CodaBox.Dal.Models
//{
//    public partial class CodaBoxContext : DbContext
//    {
//        public CodaBoxContext()
//        {
//        }

//        public CodaBoxContext(DbContextOptions<CodaBoxContext> options)
//            : base(options)
//        {
//        }

//        public virtual DbSet<Adress> Adress { get; set; }
//        public virtual DbSet<AspNetRoleClaims> AspNetRoleClaims { get; set; }
//        public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
//        public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }
//        public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }
//        public virtual DbSet<AspNetUserRoles> AspNetUserRoles { get; set; }
//        public virtual DbSet<AspNetUserTokens> AspNetUserTokens { get; set; }
//        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
//        public virtual DbSet<Company> Company { get; set; }
//        public virtual DbSet<CompteBancaire> CompteBancaire { get; set; }
//        public virtual DbSet<Country> Country { get; set; }
//        public virtual DbSet<Statements> Statements { get; set; }
//        public virtual DbSet<Transactions> Transactions { get; set; }

//        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//        {
//            if (!optionsBuilder.IsConfigured)
//            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
//                optionsBuilder.UseSqlServer("Server=.\\;Database=CodaBox;Trusted_Connection=True;");
//            }
//        }

//        protected override void OnModelCreating(ModelBuilder modelBuilder)
//        {
//            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

//            modelBuilder.Entity<Adress>(entity =>
//            {
//                entity.Property(e => e.AdressId)
//                    .HasColumnName("AdressID")
//                    .ValueGeneratedOnAdd();

//                entity.Property(e => e.AdressZipCode)
//                    .IsRequired()
//                    .HasMaxLength(16);

//                entity.Property(e => e.City)
//                    .IsRequired()
//                    .HasMaxLength(50);

//                entity.Property(e => e.CountryId).HasColumnName("CountryID");

//                entity.Property(e => e.HouseNumber)
//                    .IsRequired()
//                    .HasMaxLength(10);

//                entity.Property(e => e.Street)
//                    .IsRequired()
//                    .HasMaxLength(50);

//                entity.HasOne(d => d.AdressNavigation)
//                    .WithOne(p => p.Adress)
//                    .HasForeignKey<Adress>(d => d.AdressId)
//                    .OnDelete(DeleteBehavior.ClientSetNull)
//                    .HasConstraintName("FK_Adress_Company");

//                entity.HasOne(d => d.Country)
//                    .WithMany(p => p.Adress)
//                    .HasForeignKey(d => d.CountryId)
//                    .OnDelete(DeleteBehavior.ClientSetNull)
//                    .HasConstraintName("FK_Adress_Country");
//            });

//            modelBuilder.Entity<AspNetRoleClaims>(entity =>
//            {
//                entity.HasIndex(e => e.RoleId);

//                entity.Property(e => e.RoleId).IsRequired();

//                entity.HasOne(d => d.Role)
//                    .WithMany(p => p.AspNetRoleClaims)
//                    .HasForeignKey(d => d.RoleId);
//            });

//            modelBuilder.Entity<AspNetRoles>(entity =>
//            {
//                entity.HasIndex(e => e.NormalizedName)
//                    .HasName("RoleNameIndex")
//                    .IsUnique()
//                    .HasFilter("([NormalizedName] IS NOT NULL)");

//                entity.Property(e => e.Id).ValueGeneratedNever();

//                entity.Property(e => e.Name).HasMaxLength(256);

//                entity.Property(e => e.NormalizedName).HasMaxLength(256);
//            });

//            modelBuilder.Entity<AspNetUserClaims>(entity =>
//            {
//                entity.HasIndex(e => e.UserId);

//                entity.Property(e => e.UserId).IsRequired();

//                entity.HasOne(d => d.User)
//                    .WithMany(p => p.AspNetUserClaims)
//                    .HasForeignKey(d => d.UserId);
//            });

//            modelBuilder.Entity<AspNetUserLogins>(entity =>
//            {
//                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

//                entity.HasIndex(e => e.UserId);

//                entity.Property(e => e.LoginProvider).HasMaxLength(128);

//                entity.Property(e => e.ProviderKey).HasMaxLength(128);

//                entity.Property(e => e.UserId).IsRequired();

//                entity.HasOne(d => d.User)
//                    .WithMany(p => p.AspNetUserLogins)
//                    .HasForeignKey(d => d.UserId);
//            });

//            modelBuilder.Entity<AspNetUserRoles>(entity =>
//            {
//                entity.HasKey(e => new { e.UserId, e.RoleId });

//                entity.HasIndex(e => e.RoleId);

//                entity.HasOne(d => d.Role)
//                    .WithMany(p => p.AspNetUserRoles)
//                    .HasForeignKey(d => d.RoleId);

//                entity.HasOne(d => d.User)
//                    .WithMany(p => p.AspNetUserRoles)
//                    .HasForeignKey(d => d.UserId);
//            });

//            modelBuilder.Entity<AspNetUserTokens>(entity =>
//            {
//                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

//                entity.Property(e => e.LoginProvider).HasMaxLength(128);

//                entity.Property(e => e.Name).HasMaxLength(128);

//                entity.HasOne(d => d.User)
//                    .WithMany(p => p.AspNetUserTokens)
//                    .HasForeignKey(d => d.UserId);
//            });

//            modelBuilder.Entity<AspNetUsers>(entity =>
//            {
//                entity.HasIndex(e => e.NormalizedEmail)
//                    .HasName("EmailIndex");

//                entity.HasIndex(e => e.NormalizedUserName)
//                    .HasName("UserNameIndex")
//                    .IsUnique()
//                    .HasFilter("([NormalizedUserName] IS NOT NULL)");

//                entity.Property(e => e.Id).ValueGeneratedNever();

//                entity.Property(e => e.CompanyVat).HasColumnName("CompanyVAT");

//                entity.Property(e => e.Email).HasMaxLength(256);

//                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

//                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

//                entity.Property(e => e.UserName).HasMaxLength(256);

//                entity.HasOne(d => d.CompanyVatNavigation)
//                    .WithMany(p => p.AspNetUsers)
//                    .HasForeignKey(d => d.CompanyVat)
//                    .HasConstraintName("FK_AspNetUsers_Company");
//            });

//            modelBuilder.Entity<Company>(entity =>
//            {
//                entity.Property(e => e.CompanyName).HasMaxLength(50);

//                entity.Property(e => e.Tva)
//                    .IsRequired()
//                    .HasColumnName("TVA")
//                    .HasMaxLength(50);
//            });

//            modelBuilder.Entity<CompteBancaire>(entity =>
//            {
//                entity.Property(e => e.Bic)
//                    .IsRequired()
//                    .HasColumnName("BIc")
//                    .HasMaxLength(50);

//                entity.Property(e => e.CurrencyCode).IsRequired();

//                entity.Property(e => e.Iban)
//                    .IsRequired()
//                    .HasMaxLength(50);

//                entity.Property(e => e.IdentificationNumber).HasMaxLength(50);
//            });

//            modelBuilder.Entity<Country>(entity =>
//            {
//                entity.Property(e => e.CountryId).HasColumnName("CountryID");

//                entity.Property(e => e.CountryName)
//                    .IsRequired()
//                    .HasMaxLength(48);
//            });

//            modelBuilder.Entity<Statements>(entity =>
//            {
//                entity.HasKey(e => e.StatementId);

//                entity.Property(e => e.Date).HasColumnType("datetime");

//                entity.Property(e => e.InformationalMessage).IsRequired();

//                entity.Property(e => e.InitialBalance).HasColumnType("decimal(18, 0)");

//                entity.Property(e => e.NewBalance).HasColumnType("decimal(18, 0)");

//                entity.HasOne(d => d.CompteBancaire)
//                    .WithMany(p => p.Statements)
//                    .HasForeignKey(d => d.CompteBancaireId)
//                    .OnDelete(DeleteBehavior.ClientSetNull)
//                    .HasConstraintName("FK_Statements_Iban");
//            });

//            modelBuilder.Entity<Transactions>(entity =>
//            {
//                entity.Property(e => e.Amount).HasColumnType("decimal(18, 0)");

//                entity.Property(e => e.StructuredMessage).IsRequired();

//                entity.Property(e => e.TransactionDate).HasColumnType("datetime");

//                entity.Property(e => e.ValueDate).HasColumnType("datetime");

//                entity.HasOne(d => d.CompteBancaire)
//                    .WithMany(p => p.Transactions)
//                    .HasForeignKey(d => d.CompteBancaireId)
//                    .OnDelete(DeleteBehavior.ClientSetNull)
//                    .HasConstraintName("FK_Transactions_CompteBancaire");

//                entity.HasOne(d => d.Statement)
//                    .WithMany(p => p.Transactions)
//                    .HasForeignKey(d => d.StatementId)
//                    .OnDelete(DeleteBehavior.ClientSetNull)
//                    .HasConstraintName("FK_Transactions_Statements");
//            });
//        }
//    }
//}
