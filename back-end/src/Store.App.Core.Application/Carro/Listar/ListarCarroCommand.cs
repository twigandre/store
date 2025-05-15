using MediatR;
using Store.App.Crosscutting.Commom.ViewModel.Core.Carro.Listar;

namespace Store.App.Core.Application.Carro.Listar
{
    public class ListarCarroCommand : IRequest<ListarCarroResponse>
    {
        public ListarCarroRequest? request { get; set; }
    }
}
