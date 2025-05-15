using MediatR;
using Store.App.Core.Domain.Entitites;
using Store.App.Core.Domain.Repositories;
using System.Net;

namespace Store.App.Core.Application.Usuario.Apagar
{
    public class ApagarUsuarioHandler : IRequestHandler<ApagarUsuarioCommand, ApagarUsuarioResponse>
    {
        IUsuarioRepository _repository;
        public ApagarUsuarioHandler(IUsuarioRepository repository)
        {
            _repository = repository;
        }
        public async Task<ApagarUsuarioResponse> Handle(ApagarUsuarioCommand request, CancellationToken cancellationToken)
        {
            UsuarioEntity usuario = await _repository.Selecionar(x => x.Id == request.Id, cancellationToken: cancellationToken, "Endereco");

            if(usuario is null)
            {
                return new ApagarUsuarioResponse
                {
                    StatusCode = HttpStatusCode.NotFound,
                    TextResponse = "Usuário não encotrado!"
                };
            }

            _repository.Apagar(usuario);
            await _repository.SaveChangesAsync(cancellationToken);

            return new ApagarUsuarioResponse
            {
                StatusCode = HttpStatusCode.NoContent
            };
        }
    }
}
