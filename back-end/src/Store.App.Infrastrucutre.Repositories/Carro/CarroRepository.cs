using Microsoft.EntityFrameworkCore;
using Store.App.Core.Domain.Entitites;
using Store.App.Core.Domain.Repositories;
using Store.App.Core.Domain.Repositories.Carro;
using Store.App.Crosscutting.Commom.Security.JwtManager;
using Store.App.Crosscutting.Commom.Utils;
using Store.App.Crosscutting.Commom.ViewModel.Core.Carro.Listar;
using Store.App.Crosscutting.Commom.ViewModel.Pagination;
using Store.App.Infrastructure.Context;

namespace Store.App.Infrastrucutre.Repositories.Carro
{
    public class CarroRepository : GenericRepository<CarroEntity>, ICarroRepository
    {
        IUsuarioRepository _usuarioRepository;
        ICarroProdutoRepository _carroProdutoRepository;
        public CarroRepository(IUsuarioRepository usuarioRepository,
                               ICarroProdutoRepository carroProdutoRepository,
                               DafaultContext db_dbContext) : base(db_dbContext)
        {
            _usuarioRepository = usuarioRepository;
            _carroProdutoRepository = carroProdutoRepository;
        }

        public async Task<CarroEntity?> RecuperarCarroAtivoDoUsuarioLogado(CancellationToken cancellationToken)
        {            
            UsuarioEntity usuarioDb = await _usuarioRepository.UsuarioLogado(cancellationToken);

            CarroEntity? carroUsuario = await Selecionar(x => x.UsuarioId == usuarioDb.Id && x.Status == "em_andamento", cancellationToken, string.Empty);

            return carroUsuario;
        }

        public async Task<CarroEntity> CriarCarroParaUsuario(int idUsuario, CancellationToken cancelation)
        {
            CarroEntity carroEntity = new CarroEntity
            {
                UsuarioId = idUsuario,
                DataCriacao = DateTime.Now,
                Status = "em_andamento"
            };

            Salvar(carroEntity);
            await Context.SaveChangesAsync(cancelation);

            return carroEntity;
        }

        public async Task<CarroEntity> SelecionarCarroPorIdProduto(int idProduto, CancellationToken token) 
            => await Selecionar(x => x.Id == idProduto, token, "CarroProduto");

        public async Task<PagedItems<CarroEntity>> ListarPaginado(ListarCarroRequest request, CancellationToken cancellation)
        {
            var query = Query;

            query = query.Include(x => x.CarroProduto);

            if (request.DataInicio is not null && request.DataFim is not null)
            {
                query = query.Where(x => x.DataCriacao >= request.DataInicio && x.DataCriacao <= request.DataFim);
            }
            else
            if (request.DataInicio is not null)
            {
                query = query.Where(x => x.DataCriacao >= request.DataInicio);
            }
            else
            if (request.DataFim is not null)
            {
                query = query.Where(x => x.DataCriacao <= request.DataFim);
            }

            if (request.IdProduto != null)
            {
                var idsCarroQueTemOProduto = await _carroProdutoRepository.RecuperarIdsDeCarrosQueContemDeterminadoProduto(request.IdProduto.Value, cancellation);

                if(!idsCarroQueTemOProduto.IsNullOrEmpty())
                    query = query.Where(x => idsCarroQueTemOProduto.Contains(x.Id));
            }

            PagedItems<CarroEntity> resultadoPaginacao = await Pagination<CarroEntity>(request, query, cancellation);

            return resultadoPaginacao;
        }
                
    }
}
