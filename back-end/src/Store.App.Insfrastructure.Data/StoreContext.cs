using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.App.Infrastructure.Database.DbEntities;

namespace Store.App.Infrastructure.Database
{
    public class StoreContext : DbContext
    {
        public StoreContext(DbContextOptions<StoreContext> options) : base(options)
        {
        }
        public DbSet<CarroEntity> Carro { get; set; }
        public DbSet<CarroProdutoEntity> CarroProduto { get; set; }
        public DbSet<FilialEntity> Filial { get; set; }
        public DbSet<ProdutoEntity> Produto { get; set; }
        public DbSet<ProdutoCategoriaEntity> ProdutoCategoria { get; set; }
        public DbSet<UsuarioEntity> Usuario { get; set; }
        public DbSet<UsuarioEnderecoEntity> UsuarioEndereco { get; set; }
        public DbSet<VendaEntity> Venda { get; set; }
        public DbSet<VendaItensEntity> ItensVenda { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProdutoEntity>()
                        .ToTable("produtos");

            modelBuilder.Entity<CarroEntity>()
                        .ToTable("carro");

            modelBuilder.Entity<CarroProdutoEntity>()
                        .ToTable("carro_produtos")
                        .HasOne(n => n.Carro)
                        .WithMany(f => f.CarroProduto)
                        .HasForeignKey(n => n.CarroId)
                        .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UsuarioEntity>()
                        .ToTable("usuario");

            modelBuilder.Entity<UsuarioEnderecoEntity>()
                        .ToTable("usuario_endereco")
                        .HasOne(n => n.Usuario)
                        .WithMany(f => f.Endereco)
                        .HasForeignKey(n => n.UsuarioId)
                        .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<FilialEntity>()
                        .ToTable("filial");

            modelBuilder.Entity<ProdutoCategoriaEntity>()
                        .ToTable("produto_categoria");
            
            modelBuilder.Entity<VendaEntity>()
                        .ToTable("vendas");
            
            modelBuilder.Entity<VendaItensEntity>()
                        .ToTable("itens_venda");
        }
    }
}
