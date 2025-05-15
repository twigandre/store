using MediatR;
using Store.App.Crosscutting.Commom.Utils;
using Store.App.Infrastructure.Database.DbEntities;
using Store.App.Infrastructure.Database.DbRepository.Produto;

namespace Store.App.Core.Application.Produto.Salvar
{
    public class SalvarProdutoHandler : IRequestHandler<SalvarProdutoCommand, SalvarProdutoResponse>
    {
        IProdutoRepository _repository;
        public SalvarProdutoHandler(IProdutoRepository repository)
        {
            _repository = repository;
        }

        public async Task<SalvarProdutoResponse> Handle(SalvarProdutoCommand request, CancellationToken cancellationToken)
        {
            ProdutoEntity produtoEntity = new();

            if (request.Id != null && request.Id > 0)
            {
                produtoEntity = await _repository.Selecionar(x => x.Id == request.Id, cancellationToken, "");
            }
            else
            {
                //se for um novo produto, salva com a nota máxima.
                produtoEntity.NotaProduto = 5;
                produtoEntity.QuantidadeVendas = 0;
            }

            produtoEntity.CategoriaId = request.CategoriaId;
            produtoEntity.Descricao = request.Descricao;
            produtoEntity.Nome = request.Nome;
            produtoEntity.PrecoUnitario = request.PrecoUnitario;

            _repository.Salvar(produtoEntity);
            await _repository.Context.SaveChangesAsync(cancellationToken);

            //salvar imagem no ftp;
            string urlUploadFtp = GeneratePathUrl.GeneratePath(Directory.GetCurrentDirectory() + "," + "Ftp" + "," + produtoEntity.Id);
            Ftp.UploadFile(urlUploadFtp, request.Imagem.Hash, request.Imagem.Nome);

            return new SalvarProdutoResponse
            {
                Id = produtoEntity.Id
            };
        }
    }
}
