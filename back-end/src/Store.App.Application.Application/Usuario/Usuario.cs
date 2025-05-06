using Store.App.Crosscutting.Commom.Security;
using Store.App.Crosscutting.Commom.Security.JwtManager;
using Store.App.Crosscutting.Commom.ViewModel;
using Store.App.Crosscutting.Commom.ViewModel.Login;
using Store.App.Infrastructure.Database.DbRepository.Usuario;
using Store.App.Infrastructure.Database.Entities;

namespace Store.App.Core.Application.Usuario
{
    public class Usuario : IUsuario
    {
        IUsuarioRepository _repository;
        IJwtManager _jwt;

        public Usuario(IUsuarioRepository repository,
                       IJwtManager jwt)
        {
            _repository = repository;
            _jwt = jwt;
        }

        public RequestResponse Logar(LoginRequest obj)
        {
            UsuarioEntity usuario = _repository.Selecionar(x => x.Email.ToUpper().Equals(obj.username.ToUpper()), "Nome");

            string senhaEncript = HashPassword.StringToHash(obj.password);

            if (usuario is null || !usuario.Senha.Equals(senhaEncript))
            {
                return new RequestResponse
                {
                    TextResponse = "Login ou senha inválidos",
                    StatusCode = System.Net.HttpStatusCode.NotFound
                };
            }

            User user = new User
            {
                Name = usuario.Nome.Nome.ToUpper() + " " + usuario.Nome.SobreNome.ToUpper(),
                Role = usuario.Perfil,
                NameId = usuario.Email.ToUpper()
            };

            string token = _jwt.GenerateToken(user);

            return new RequestResponse
            {
                TextResponse = token
            };
        }
    }
}
