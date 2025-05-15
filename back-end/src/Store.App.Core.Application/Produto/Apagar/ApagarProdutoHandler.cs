using MediatR;
using Store.App.Core.Domain.Entitites;
using Store.App.Core.Domain.Repositories;
using Store.App.Core.Domain.Repositories.Carro;

namespace Store.App.Core.Application.Produto.Apagar
{
    public class ApagarProdutoHandler : IRequestHandler<ApagarProdutoCommand, ApagarProdutoResponse>
    {
        IProdutoRepository _produtoRepository;
        ICarroRepository _carroRepository;    
        public ApagarProdutoHandler(IProdutoRepository produtoRepository, ICarroRepository carroRepository)
        {
            _produtoRepository = produtoRepository;
            _carroRepository = carroRepository;
        }

        public async Task<ApagarProdutoResponse> Handle(ApagarProdutoCommand request, CancellationToken cancellationToken)
        {
            ProdutoEntity produtoEntity = await _produtoRepository.Selecionar(x => x.Id == request.Id, cancellationToken, "Carro");

            if(produtoEntity.Carro.Count > 0)
            {
                _carroRepository.RemoveRange(produtoEntity.Carro);
                await _carroRepository.SaveChangesAsync(cancellationToken);
            }

            _produtoRepository.Apagar(new ProdutoEntity { Id = request.Id });
            await _produtoRepository.SaveChangesAsync(cancellationToken);

            return new ApagarProdutoResponse();
        }
    }
}
