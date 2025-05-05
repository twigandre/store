using Microsoft.Extensions.DependencyInjection;
using Store.App.Crosscutting.Commom.Security.JwtManager;
using Store.App.Infrastructure.Database.DbRepository;
using Store.App.Infrastructure.Database.DbRepository.Cliente;
using Store.App.Infrastructure.Database.DbRepository.Filial;
using Store.App.Infrastructure.Database.DbRepository.ItensVenda;
using Store.App.Infrastructure.Database.DbRepository.Produto;
using Store.App.Infrastructure.Database.DbRepository.ProdutoCategoria;
using Store.App.Infrastructure.Database.DbRepository.Usuario;
using Store.App.Infrastructure.Database.DbRepository.Venda;
using Store.App.Infrastructure.Database.Entities;
using Store.App.Infrastructure.Database.Entitites;

namespace Store.App.Crosscutting.IoC.DependencyInjection
{
    public static class DependencyInjection
    {
        public static void Initialize(this IServiceCollection services) =>
            services.Services().
                     Entities().
                     Repositories().
                     Security();

        private static IServiceCollection Services(this IServiceCollection services)
        {

            return services;
        }

        private static IServiceCollection Entities(this IServiceCollection services)
        {
            services.AddScoped<IGenericRepository<ClienteEntity>, GenericRepository<ClienteEntity>>();
            services.AddScoped<IGenericRepository<FilialEntity>, GenericRepository<FilialEntity>>();
            services.AddScoped<IGenericRepository<ItensVendaEntity>, GenericRepository<ItensVendaEntity>>();
            services.AddScoped<IGenericRepository<ProdutoCategoriaEntity>, GenericRepository<ProdutoCategoriaEntity>>();
            services.AddScoped<IGenericRepository<ProdutoEntity>, GenericRepository<ProdutoEntity>>();
            services.AddScoped<IGenericRepository<UsuarioEntity>, GenericRepository<UsuarioEntity>>();
            services.AddScoped<IGenericRepository<VendaEntity>, GenericRepository<VendaEntity>>();

            return services;
        }

        private static IServiceCollection Repositories(this IServiceCollection services)
        {
            services.AddScoped<IClienteRepository, ClienteRepository>();
            services.AddScoped<IFilialRepository, FilialRepository>();
            services.AddScoped<IItensVendaRepository, ItensVendaRepository>();
            services.AddScoped<IProdutoRepository, ProdutoRepository>();
            services.AddScoped<IProdutoCategoriaRepository, ProdutoCategoriaRepository>();
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped<IVendaRepository, VendaRepository>();

            return services;
        }

        private static IServiceCollection Security(this IServiceCollection services)
        {
            services.AddScoped<IJwtManager, JwtManager>();

            return services;
        }
    }
}
