using Microsoft.EntityFrameworkCore;
using Store.App.Infrastructure.Database.Entities;

namespace Store.App.Infrastructure.Database
{
    public class StoreContext : DbContext
    {
        public StoreContext(DbContextOptions<StoreContext> options) : base(options)
        {
        }

        public DbSet<ClienteEntity> Cliente { get; set; }
        public DbSet<FilialEntity> Filial { get; set; }
        public DbSet<ItensVendaEntity> ItensVenda { get; set; }
        public DbSet<ProdutoEntity> Produto { get; set; }
        public DbSet<UsuarioEntity> Usuario { get; set; }
        public DbSet<VendaEntity> Venda { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ClienteEntity>()
            .Property(s => s.Id)
            .HasColumnName("id")
            .HasDefaultValue(0)
            .IsRequired();
            
            modelBuilder.Entity<FilialEntity>()
            .Property(s => s.Id)
            .HasColumnName("id")
            .HasDefaultValue(0)
            .IsRequired();
            
            modelBuilder.Entity<ItensVendaEntity>()
            .Property(s => s.Id)
            .HasColumnName("id")
            .HasDefaultValue(0)
            .IsRequired(); 
            
            modelBuilder.Entity<ProdutoEntity>()
            .Property(s => s.Id)
            .HasColumnName("id")
            .HasDefaultValue(0)
            .IsRequired();
            
            modelBuilder.Entity<UsuarioEntity>()
            .Property(s => s.Id)
            .HasColumnName("id")
            .HasDefaultValue(0)
            .IsRequired(); 
            
            modelBuilder.Entity<VendaEntity>()
            .Property(s => s.Id)
            .HasColumnName("id")
            .HasDefaultValue(0)
            .IsRequired();
        }
    }
}
