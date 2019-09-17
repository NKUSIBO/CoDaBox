
using System.IO;
using Inocrea.CodaBox.ApiModel;
using Inocrea.CodaBox.ApiModel.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Inocrea.CodaBox.ApiServer.Entities
{
    public partial class DbInosalesContext : DbContext
    {
        public DbInosalesContext()
        {
        }

        public DbInosalesContext(DbContextOptions<DbInosalesContext> options)
            : base(options)
        {
        }

        //public virtual DbSet<AspNetRoleClaims> AspNetRoleClaims { get; set; }
        //public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        //public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }
        //public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }
        //public virtual DbSet<AspNetUserRoles> AspNetUserRoles { get; set; }
        //public virtual DbSet<Entities2.AspNetUserTokens> AspNetUserTokens { get; set; }
        //public virtual DbSet<Entities2.AspNetUsers> AspNetUsers { get; set; }
        public virtual DbSet<CodaIdentities> CodaIdentities { get; set; }
        public virtual DbSet<Company> Company { get; set; }
        public virtual DbSet<CompteBancaire> CompteBancaire { get; set; }
    
        public virtual DbSet<Statements> Statements { get; set; }
        public virtual DbSet<Transactions> Transactions { get; set; }

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

            //modelBuilder.Entity<AspNetRoleClaims>(entity =>
            //{
            //    entity.HasIndex(e => e.RoleId);

            //    entity.Property(e => e.RoleId).IsRequired();

            //    entity.HasOne(d => d.Role)
            //        .WithMany(p => p.AspNetRoleClaims)
            //        .HasForeignKey(d => d.RoleId);
            //});

            //modelBuilder.Entity<AspNetRoles>(entity =>
            //{
            //    entity.HasIndex(e => e.NormalizedName)
            //        .HasName("RoleNameIndex")
            //        .IsUnique()
            //        .HasFilter("([NormalizedName] IS NOT NULL)");

            //    entity.Property(e => e.Id).ValueGeneratedNever();

            //    entity.Property(e => e.Name).HasMaxLength(256);

            //    entity.Property(e => e.NormalizedName).HasMaxLength(256);
            //});

            //modelBuilder.Entity<AspNetUserClaims>(entity =>
            //{
            //    entity.HasIndex(e => e.UserId);

            //    entity.Property(e => e.UserId).IsRequired();

            //    entity.HasOne(d => d.User)
            //        .WithMany(p => p.AspNetUserClaims)
            //        .HasForeignKey(d => d.UserId);
            //});

            //modelBuilder.Entity<AspNetUserLogins>(entity =>
            //{
            //    entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

            //    entity.HasIndex(e => e.UserId);

            //    entity.Property(e => e.UserId).IsRequired();

            //    entity.HasOne(d => d.User)
            //        .WithMany(p => p.AspNetUserLogins)
            //        .HasForeignKey(d => d.UserId);
            //});

            //modelBuilder.Entity<AspNetUserRoles>(entity =>
            //{
            //    entity.HasKey(e => new { e.UserId, e.RoleId });

            //    entity.HasIndex(e => e.RoleId);

            //    entity.HasOne(d => d.Role)
            //        .WithMany(p => p.AspNetUserRoles)
            //        .HasForeignKey(d => d.RoleId);

            //    entity.HasOne(d => d.User)
            //        .WithMany(p => p.AspNetUserRoles)
            //        .HasForeignKey(d => d.UserId);
            //});

            //modelBuilder.Entity<AspNetUserTokens>(entity =>
            //{
            //    entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

            //    entity.HasOne(d => d.User)
            //        .WithMany(p => p.AspNetUserTokens)
            //        .HasForeignKey(d => d.UserId);
            //});

            //modelBuilder.Entity<AspNetUsers>(entity =>
            //{
            //    entity.HasIndex(e => e.CompanyId);

            //    entity.HasIndex(e => e.NormalizedEmail)
            //        .HasName("EmailIndex");

            //    entity.HasIndex(e => e.NormalizedUserName)
            //        .HasName("UserNameIndex")
            //        .IsUnique()
            //        .HasFilter("([NormalizedUserName] IS NOT NULL)");

            //    entity.Property(e => e.Id).ValueGeneratedNever();

            //    entity.Property(e => e.Email).HasMaxLength(256);

            //    entity.Property(e => e.FirstName).HasMaxLength(200);

            //    entity.Property(e => e.LastName).HasMaxLength(250);

            //    entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

            //    entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

            //    entity.Property(e => e.UserName).HasMaxLength(256);

            //    entity.HasOne(d => d.Company)
            //        .WithMany(p => p.AspNetUsers)
            //        .HasForeignKey(d => d.CompanyId);
            //});

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
