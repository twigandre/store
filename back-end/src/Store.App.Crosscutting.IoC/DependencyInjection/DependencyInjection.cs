using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Store.App.Core.Domain.Repositories;
using Store.App.Core.Domain.Repositories.Carro;
using Store.App.Core.Domain.Repositories.Venda;
using Store.App.Crosscutting.Commom.Security.JwtManager;
using Store.App.Infrastrucutre.Repositories.Carro;
using Store.App.Infrastrucutre.Repositories.Carro.CarroProduto;
using Store.App.Infrastrucutre.Repositories.Filial;
using Store.App.Infrastrucutre.Repositories.ItensVenda;
using Store.App.Infrastrucutre.Repositories.Produto;
using Store.App.Infrastrucutre.Repositories.ProdutoCategoria;
using Store.App.Infrastrucutre.Repositories.Usuario;
using Store.App.Infrastrucutre.Repositories.UsuarioEndereco;
using Store.App.Infrastrucutre.Repositories.Venda;

namespace Store.App.Crosscutting.IoC.DependencyInjection
{
    public static class DependencyInjection
    {
        public static void Initialize(this IServiceCollection services) =>
            services.Security().
                     Services().
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

        private static IServiceCollection Repositories(this IServiceCollection services)
        {
            services.AddScoped<IFilialRepository, FilialRepository>();
            services.AddScoped<IItensVendaRepository, ItensVendaRepository>();
            services.AddScoped<IProdutoRepository, ProdutoRepository>();
            services.AddScoped<IProdutoCategoriaRepository, ProdutoCategoriaRepository>();
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped<IUsuarioEnderecoRepository, UsuarioEnderecoRepository>();
            services.AddScoped<IVendaRepository, VendaRepository>();
            services.AddScoped<ICarroRepository, CarroRepository>();
            services.AddScoped<ICarroProdutoRepository, CarroProdutoRepository>();

            return services;
        }

       
    }
}
