using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.App.Infrastructure.Database.Entities;

namespace Store.App.Infrastructure.Database
{
    public class StoreContext : DbContext
    {
        public StoreContext(DbContextOptions<StoreContext> options) : base(options)
        {
        }
        public DbSet<CarroEntity> Carro { get; set; }
        public DbSet<CarroProdutoEntity> CarroProduto { get; set; }
        public DbSet<ClienteEntity> Cliente { get; set; }
        public DbSet<FilialEntity> Filial { get; set; }
        public DbSet<ProdutoEntity> Produto { get; set; }
        public DbSet<ProdutoCategoriaEntity> ProdutoCategoria { get; set; }
        public DbSet<UsuarioEntity> Usuario { get; set; }
        public DbSet<UsuarioNomeEntity> UsuarioNome { get; set; }
        public DbSet<UsuarioEnderecoEntity> UsuarioEndereco { get; set; }
        public DbSet<VendaEntity> Venda { get; set; }
        public DbSet<VendaItensEntity> ItensVenda { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            DefaultProperty<CarroEntity>(modelBuilder.Entity<CarroEntity>());
            DefaultProperty<CarroProdutoEntity>(modelBuilder.Entity<CarroProdutoEntity>());
            DefaultProperty<ClienteEntity>(modelBuilder.Entity<ClienteEntity>());
            DefaultProperty<FilialEntity>(modelBuilder.Entity<FilialEntity>());
            DefaultProperty<ProdutoEntity>(modelBuilder.Entity<ProdutoEntity>());
            DefaultProperty<ProdutoCategoriaEntity>(modelBuilder.Entity<ProdutoCategoriaEntity>());
            DefaultProperty<UsuarioEntity>(modelBuilder.Entity<UsuarioEntity>());
            DefaultProperty<UsuarioNomeEntity>(modelBuilder.Entity<UsuarioNomeEntity>());
            DefaultProperty<UsuarioEnderecoEntity>(modelBuilder.Entity<UsuarioEnderecoEntity>());
            DefaultProperty<VendaEntity>(modelBuilder.Entity<VendaEntity>());
            DefaultProperty<VendaItensEntity>(modelBuilder.Entity<VendaItensEntity>());

        }

        private PropertyBuilder DefaultProperty<T>(EntityTypeBuilder entity, string pkNameInDb = "id")
        {
            var props = typeof(T)
                       .GetProperties()
                       .Where(prop => Attribute.IsDefined(prop, typeof(System.ComponentModel.DataAnnotations.KeyAttribute)));

            return entity.Property(props.FirstOrDefault().Name)
                         .HasColumnName(pkNameInDb)
                         .HasDefaultValue(0)
                         .IsRequired();
        }
    }
}
