using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Database.Models
{
    public partial class IbankingContext : IdentityDbContext
    {
        public IbankingContext()
        {
        }

        public IbankingContext(DbContextOptions<IbankingContext> options)
            : base(options)
        {
        }
        public virtual DbSet<Usuario> Usuarios { get; set; }
        public virtual DbSet<Productos> Productos { get; set; }
        public virtual DbSet<Transacciones> Transacciones { get; set; }
        public virtual DbSet<Beneficiario> Beneficiarios { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {

                optionsBuilder.UseSqlServer("Server=LAPTOP-184T66BD\\SQLEXPRESS;Database=Ibanking;Trusted_Connection=True;");
            }
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            OnModelCreatingPartial(modelBuilder);

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Estado).HasDefaultValue("Activo");

            });

            modelBuilder.Entity<Beneficiario>(entity =>
            {
                entity.HasKey(e => e.Id);

            });


            modelBuilder.Entity<Productos>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Id);

            });

            modelBuilder.Entity<Transacciones>(entity =>
            {
                entity.HasKey(e => e.Id);

            });
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
