using MediatR;
using Store.App.Crosscutting.Commom.ViewModel.Pagination;
using Store.App.Crosscutting.Commom.ViewModel.Core.Listar;
using Store.App.Crosscutting.Commom.ViewModel.Core.Produto.Listar;
using Store.App.Crosscutting.Commom.ViewModel.Core.Categoria;
using Store.App.Crosscutting.Commom.ViewModel.Core.Produto;
using Store.App.Core.Domain.Repositories;

namespace Store.App.Core.Application.Produto.Listar
{
    public class ListarProdutoHandler : IRequestHandler<ListarProdutoCommand, ListarProdutoResponse>
    {
        IProdutoRepository _repository;
        public ListarProdutoHandler(IProdutoRepository repository)
        {
            _repository = repository;
        }

        public async Task<ListarProdutoResponse> Handle(ListarProdutoCommand obj, CancellationToken cancellationToken)
        {
            var filter = obj.request is null ? new ListarProdutoRequest() : obj.request;

            var resultadoPaginado = await _repository.ListarUsuariosPaginado(filter, cancellationToken);

            if (resultadoPaginado.Total == 0)
            {
                return new ListarProdutoResponse
                {
                    data = new PagedItems<ListarProdutoResult>
                    {
                        Total = 0,
                        Items = null
                    }
                };
            }

            return new ListarProdutoResponse
            {
                data = new PagedItems<ListarProdutoResult>
                {
                    Total = resultadoPaginado.Total,
                    Items = resultadoPaginado.Items.Select(x => new ListarProdutoResult
                    {
                       Categoria = new CategoriaVM
                       {
                           Nome = x.Categoria.Nome,
                           Id = x.Categoria.Id
                       },
                       CategoriaId = x.CategoriaId,
                       Descricao = x.Descricao,
                       Nome = x.Nome,
                       Id = x.Id,
                       PrecoUnitario = x.PrecoUnitario,
                       Imagem = FtpProduto.DownloadArquivo(x.Id),
                       Avaliacao = new AvaliacaoVM
                       {
                           Nota = x.NotaProduto,
                           NumeroVendas = x.QuantidadeVendas
                       }
                    }).ToList()
                }
            };
        }
    }
}
