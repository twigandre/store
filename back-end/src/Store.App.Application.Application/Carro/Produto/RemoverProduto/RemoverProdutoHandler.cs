using MediatR;
using Store.App.Crosscutting.Commom.ViewModel;
using Store.App.Crosscutting.Commom.ViewModel.Core.Carro;
using Store.App.Infrastructure.Database.DbEntities;
using Store.App.Infrastructure.Database.DbRepository.Carro;
using Store.App.Insfrastructure.Database.DbRepository.Carro.CarroProduto;

namespace Store.App.Core.Application.Carro.Produto.RemoverProduto
{
    public class RemoverProdutoHandler : IRequestHandler<RemoverProdutoCommand, RemoverProdutoResult>
    {
        ICarroRepository _repositoryCarro;
        ICarroProdutoRepository _repositoryCarroProduto;

        public RemoverProdutoHandler(ICarroRepository repositoryCarro,
                                     ICarroProdutoRepository repositoryCarroProduto)
        {
            _repositoryCarro = repositoryCarro;
            _repositoryCarroProduto = repositoryCarroProduto;
        }

        public async Task<RemoverProdutoResult> Handle(RemoverProdutoCommand request, CancellationToken cancellationToken)
        {
            CarroEntity? carro = await _repositoryCarro.RecuperarCarroAtivoDoUsuarioLogado(cancellationToken);

            if(carro is null)
            {
                return new RemoverProdutoResult
                {
                    TextResponse = "Falha ao recuperar carro do usuário logado.",
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };
            }

            CarroProdutoEntity carroProduto = await _repositoryCarroProduto.Selecionar(x => x.CarroId == carro.Id && x.ProdutoId == request.IdProduto, cancellationToken);

            if(carroProduto is null)
            {
                return new RemoverProdutoResult
                {
                    TextResponse = "Produto não está vinculado ao carro.",
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };
            }

            _repositoryCarroProduto.Apagar(carroProduto);
            await _repositoryCarroProduto.Context.SaveChangesAsync(cancellationToken);

            return new RemoverProdutoResult
            {
                TextResponse = "Produto removido do carro com sucesso.",
                StatusCode = System.Net.HttpStatusCode.OK
            };
        }
    }
}
