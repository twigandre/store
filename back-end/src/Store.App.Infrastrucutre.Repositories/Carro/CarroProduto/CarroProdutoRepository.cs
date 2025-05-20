using Store.App.Crosscutting.Commom.ViewModel;
using Store.App.Crosscutting.Commom.ViewModel.Core.Carro;
using Microsoft.EntityFrameworkCore;
using Store.App.Crosscutting.Commom.Utils;
using Store.App.Core.Domain.Entitites;
using Store.App.Core.Domain.Repositories;
using Store.App.Infrastructure.Context;

namespace Store.App.Infrastrucutre.Repositories.Carro.CarroProduto
{
    public class CarroProdutoRepository : GenericRepository<CarroProdutoEntity>, ICarroProdutoRepository
    {
        IProdutoRepository _produtoRepository;
        public CarroProdutoRepository(IProdutoRepository produtoRepository, DafaultContext db_dbContext) : base(db_dbContext)
        {
            _produtoRepository = produtoRepository;
        }

        public async Task<RequestResponseVM> IncluirProdutoNoCarro(ManterCarroProdutoVM obj, CancellationToken cancellationToken)
        {
            CarroProdutoEntity? carroProdutoEntity = await Selecionar(x => x.CarroId == obj.IdCarro && x.ProdutoId == obj.IdProduto, cancellationToken, string.Empty);   

            if(carroProdutoEntity is null) 
            {
                carroProdutoEntity = new CarroProdutoEntity();
                carroProdutoEntity.Quantidade = 0;
            }

            int qtdProduto = (carroProdutoEntity.Quantidade + 1);

            if (qtdProduto > 20)
            {
                var produto = await _produtoRepository.Selecionar(x => x.Id == obj.IdProduto, cancellationToken, string.Empty);
                
                return new RequestResponseVM
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    TextResponse = "O Produto " + produto.Nome + " ultrapassou a quatidade limite de compra. apenas 20 Unidades por carro!"
                };
            }

            carroProdutoEntity.Quantidade = qtdProduto;
            carroProdutoEntity.CarroId = obj.IdCarro;
            carroProdutoEntity.ProdutoId = obj.IdProduto;
            carroProdutoEntity.DataHoraUltimaAlteracao = DateTime.Now;

            Salvar(carroProdutoEntity);
            await Context.SaveChangesAsync(cancellationToken);

            return new RequestResponseVM { 
                TextResponse = "Produto adicionado com sucesso ao carro!"
            };
        }

        public async Task<RequestResponseVM> RemoverProdutoDoCarro(ManterCarroProdutoVM obj, CancellationToken cancellationToken)
        {
            CarroProdutoEntity? carroProdutoEntity = await Selecionar(x => x.CarroId == obj.IdCarro && x.ProdutoId == obj.IdProduto, cancellationToken, string.Empty);       
            
            if(carroProdutoEntity is null)
            {
                return new RequestResponseVM
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    TextResponse = "Produto não encontrado no carro!"
                };
            }

            if(carroProdutoEntity.Quantidade == 1)
            {
                //se remover, com somente 1 produto, remove o produto do carro.
                Apagar(carroProdutoEntity);
            }
            else
            {
                carroProdutoEntity.Quantidade = carroProdutoEntity.Quantidade - 1;
                carroProdutoEntity.DataHoraUltimaAlteracao = DateTime.Now;

                Salvar(carroProdutoEntity);
            }

            await SaveChangesAsync(cancellationToken);

            return new RequestResponseVM
            {
                TextResponse = "Produto removido com sucesso do carro!"
            };
        }

        public async Task<List<int>> RecuperarIdsDeCarrosQueContemDeterminadoProduto(int idProduto, CancellationToken cancellationToken)
        {
            var query = Query;

            query = query.Include(x => x.Carro);

            query = query.Select(x => new CarroProdutoEntity
            {
                CarroId = x.CarroId
            });

            query = query.Where(x => x.ProdutoId == idProduto);

            var listaEntity = await query.AsNoTracking().ToListAsync(cancellationToken);

            if (listaEntity.IsNullOrEmpty())
            {
                return new List<int>(); 
            }

            return listaEntity.Select(x => x.CarroId).ToList();
        }

    }
}
