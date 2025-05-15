using MediatR;
using Store.App.Crosscutting.Commom.ViewModel.Core.Produto.Listar;

namespace Store.App.Core.Application.Produto.Listar
{
    public class ListarProdutoCommand : IRequest<ListarProdutoResponse>
    {
        public ListarProdutoRequest? request { get; set; }
    }
}
