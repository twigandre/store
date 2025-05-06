using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Store.App.Crosscutting.Commom.Security.JwtManager;
using Store.App.Infrastructure.Database.DbRepository;
using Store.App.Infrastructure.Database.DbRepository.Carro;
using Store.App.Infrastructure.Database.DbRepository.Cliente;
using Store.App.Infrastructure.Database.DbRepository.Filial;
using Store.App.Infrastructure.Database.DbRepository.ItensVenda;
using Store.App.Infrastructure.Database.DbRepository.Produto;
using Store.App.Infrastructure.Database.DbRepository.ProdutoCategoria;
using Store.App.Infrastructure.Database.DbRepository.Usuario;
using Store.App.Infrastructure.Database.DbRepository.Venda;
using Store.App.Infrastructure.Database.Entities;

namespace Store.App.Crosscutting.IoC.DependencyInjection
{
    public static class DependencyInjection
    {
        public static void Initialize(this IServiceCollection services) =>
            services.Security().
                     Services().
                     Entities().
                     Repositories();
        private static IServiceCollection Security(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IJwtManager, JwtManager>();

            return services;
        }

        private static IServiceCollection Services(this IServiceCollection services)
        {
            return services;
        }

        private static IServiceCollection Entities(this IServiceCollection services)
        {
            services.AddScoped<IGenericRepository<CarroEntity>, GenericRepository<CarroEntity>>();
            services.AddScoped<IGenericRepository<CarroProdutoEntity>, GenericRepository<CarroProdutoEntity>>();
            services.AddScoped<IGenericRepository<ClienteEntity>, GenericRepository<ClienteEntity>>();
            services.AddScoped<IGenericRepository<FilialEntity>, GenericRepository<FilialEntity>>();
            services.AddScoped<IGenericRepository<ProdutoEntity>, GenericRepository<ProdutoEntity>>();
            services.AddScoped<IGenericRepository<ProdutoCategoriaEntity>, GenericRepository<ProdutoCategoriaEntity>>();       
            services.AddScoped<IGenericRepository<UsuarioEntity>, GenericRepository<UsuarioEntity>>();
            services.AddScoped<IGenericRepository<UsuarioNomeEntity>, GenericRepository<UsuarioNomeEntity>>();
            services.AddScoped<IGenericRepository<UsuarioEnderecoEntity>, GenericRepository<UsuarioEnderecoEntity>>();
            services.AddScoped<IGenericRepository<VendaEntity>, GenericRepository<VendaEntity>>();
            services.AddScoped<IGenericRepository<VendaItensEntity>, GenericRepository<VendaItensEntity>>();

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
            services.AddScoped<ICarroRepository, CarroRepository>();

            return services;
        }

       
    }
}
