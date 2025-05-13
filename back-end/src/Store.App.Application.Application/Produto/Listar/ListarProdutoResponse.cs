using MediatR;
using Store.App.Crosscutting.Commom.ViewModel.Core.Listar;
using Store.App.Crosscutting.Commom.ViewModel.Pagination;

namespace Store.App.Core.Application.Produto.Listar
{
    public class ListarProdutoResponse : IRequest<ListarProdutoResult>
    {
        public PagedItems<ListarProdutoResult>? data { get; set; }
    }
}
