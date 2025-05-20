using MediatR;
using Store.App.Core.Domain.Entitites;
using Store.App.Core.Domain.Repositories;
using Store.App.Core.Domain.Repositories.Carro;

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

            CarroProdutoEntity carroProduto = await _repositoryCarroProduto.Selecionar(x => x.CarroId == carro.Id && x.ProdutoId == request.IdProduto, cancellationToken, string.Empty);

            if(carroProduto is null)
            {
                return new RemoverProdutoResult
                {
                    TextResponse = "Produto não está vinculado ao carro.",
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };
            }

            _repositoryCarroProduto.Apagar(carroProduto);
            await _repositoryCarroProduto.SaveChangesAsync(cancellationToken);

            return new RemoverProdutoResult
            {
                TextResponse = "Produto removido do carro com sucesso.",
                StatusCode = System.Net.HttpStatusCode.OK
            };
        }
    }
}
