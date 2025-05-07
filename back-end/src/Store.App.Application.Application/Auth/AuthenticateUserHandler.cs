using MediatR;
using Store.App.Crosscutting.Commom.Security;
using Store.App.Crosscutting.Commom.Security.JwtManager;
using Store.App.Infrastructure.Database.DbRepository.Usuario;
using Store.App.Infrastructure.Database.DbEntities;
using System.Net;

namespace Store.App.Core.Application.Auth
{
    public class AuthenticateUserHandler : IRequestHandler<AuthenticateUserCommand, AuthenticateUserResult>
    {
        IUsuarioRepository _repository;
        IJwtManager _jwt;
        public AuthenticateUserHandler(IUsuarioRepository repository,
                                       IJwtManager jwt)
        {
            _repository = repository;
            _jwt = jwt;
        }

        public async Task<AuthenticateUserResult> Handle(AuthenticateUserCommand request, CancellationToken cancellationToken)
        {
            UsuarioEntity usuario = await _repository.Selecionar(x => x.Email.ToUpper().Equals(request.email.ToUpper()), "", cancellationToken);

            string senhaEncript = HashPassword.StringToHash(request.password);

            if (usuario is null || !usuario.Senha.Equals(senhaEncript))
            {
                return new AuthenticateUserResult
                {
                    TextResponse = "Login ou senha inválidos",
                    StatusCode = HttpStatusCode.NotFound,
                };
            }

            User user = new User
            {
                Name = usuario.PrimeiroNome.ToUpper() + " " + usuario.SobreNome.ToUpper(),
                Role = usuario.Perfil,
                NameId = usuario.Email.ToUpper()
            };

            return new AuthenticateUserResult
            {
                Token = _jwt.GenerateToken(user),
                TextResponse = "Login realizado com sucesso!",
                StatusCode = HttpStatusCode.OK
            };
        }
    }
}
