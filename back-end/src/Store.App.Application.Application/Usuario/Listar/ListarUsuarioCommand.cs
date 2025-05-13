using MediatR;
using Store.App.Crosscutting.Commom.ViewModel.Core.Usuario.Listar;

namespace Store.App.Core.Application.Usuario.Listar
{
    public class ListarUsuarioCommand : IRequest<ListarUsuarioResponse>
    {
        public ListarUsuarioRequest? obj { get; set; }
    }
}
