using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace JMusik.Data
{
    public partial class TiendaDbContext : DbContext
    {
        public TiendaDbContext()
        {
        }

        public TiendaDbContext(DbContextOptions<TiendaDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<DetalleOrden> DetalleOrdens { get; set; }
        public virtual DbSet<Orden> Ordens { get; set; }
        public virtual DbSet<Perfil> Perfils { get; set; }
        public virtual DbSet<Producto> Productos { get; set; }
        public virtual DbSet<Usuario> Usuarios { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=localhost;Initial Catalog=TiendaDb;Integrated Security=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Modern_Spanish_CI_AS");

            modelBuilder.Entity<DetalleOrden>(entity =>
            {
                entity.ToTable("DetalleOrden", "tienda");

                entity.HasIndex(e => e.OrdenId, "IX_DetalleOrden_OrdenId");

                entity.HasIndex(e => e.ProductoId, "IX_DetalleOrden_ProductoId");

                entity.Property(e => e.Cantidad).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.PrecioUnitario).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Total).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.Orden)
                    .WithMany(p => p.DetalleOrdens)
                    .HasForeignKey(d => d.OrdenId);

                entity.HasOne(d => d.Producto)
                    .WithMany(p => p.DetalleOrdens)
                    .HasForeignKey(d => d.ProductoId);
            });

            modelBuilder.Entity<Orden>(entity =>
            {
                entity.ToTable("Orden", "tienda");

                entity.HasIndex(e => e.UsuarioId, "IX_Orden_UsuarioId");

                entity.Property(e => e.CantidadArticulos).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Importe).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.Usuario)
                    .WithMany(p => p.Ordens)
                    .HasForeignKey(d => d.UsuarioId);
            });

            modelBuilder.Entity<Perfil>(entity =>
            {
                entity.ToTable("Perfil", "tienda");

                entity.Property(e => e.Nombre).HasMaxLength(50);
            });

            modelBuilder.Entity<Producto>(entity =>
            {
                entity.ToTable("Producto", "tienda");

                entity.Property(e => e.Nombre).HasMaxLength(256);

                entity.Property(e => e.Precio).HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.ToTable("Usuario", "tienda");

                entity.HasIndex(e => e.PerfilId, "IX_Usuario_PerfilId");

                entity.Property(e => e.Apellidos).HasMaxLength(256);

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.Nombre).HasMaxLength(50);

                entity.Property(e => e.Password).HasMaxLength(512);

                entity.Property(e => e.Username).HasMaxLength(25);

                entity.HasOne(d => d.Perfil)
                    .WithMany(p => p.Usuarios)
                    .HasForeignKey(d => d.PerfilId);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
