using MediatR;
using Store.App.Infrastructure.Database.DbEntities;
using Store.App.Infrastructure.Database.DbRepository.Carro;
using Store.App.Infrastructure.Database.DbRepository.Produto;

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
                await _carroRepository.Context.SaveChangesAsync(cancellationToken);
            }

            _produtoRepository.Apagar(new ProdutoEntity { Id = request.Id });
            await _produtoRepository.Context.SaveChangesAsync(cancellationToken);

            return new ApagarProdutoResponse();
        }
    }
}
