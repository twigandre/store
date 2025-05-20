using Bogus;
using Moq;
using Store.App.Core.Application.Auth;
using Store.App.Core.Domain.Entitites;
using Store.App.Core.Domain.Repositories;
using Store.App.Crosscutting.Commom.Security.JwtManager;
using Store.App.Crosscutting.Commom.Security;
using System.Net;

namespace TestesUnitarios.Core.Application.Auth
{
    public class Auth
    {
        private readonly Mock<IUsuarioRepository> _repositoryMock;
        private readonly Mock<IJwtManager> _jwtManagerMock;
        private readonly AuthenticateUserHandler _handler;
        private readonly Faker faker;

        public Auth()
        {
            _repositoryMock = new Mock<IUsuarioRepository>();
            _jwtManagerMock = new Mock<IJwtManager>();
            _handler = new AuthenticateUserHandler(_repositoryMock.Object, _jwtManagerMock.Object);
            faker = new Faker("pt_BR");
        }

        [Fact]
        public async Task UsuarioNaoExisteNaBase()
        {
            // Arrange
            var command = new AuthenticateUserCommand
            {
                email = faker.Internet.Email(),
                password = faker.Internet.Password()
            };

            _repositoryMock.Setup(r => r.Selecionar(
                It.IsAny<System.Linq.Expressions.Expression<System.Func<UsuarioEntity, bool>>>(),
                It.IsAny<CancellationToken>(),
                It.IsAny<string>()))
                .ReturnsAsync((UsuarioEntity)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal("Login ou senha inválidos", result.TextResponse);
            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
            Assert.Null(string.IsNullOrEmpty(result.Token) ? null : result.Token);
        }

        [Fact]
        public async Task SenhaIncorreta()
        {
            // Arrange
            var passwordCorreto = faker.Internet.Password();
            var commandPasswordErrado = faker.Internet.Password();

            var usuario = new UsuarioEntity
            {
                Email = faker.Internet.Email(),
                Senha = HashPassword.StringToHash(passwordCorreto),
                PrimeiroNome = faker.Name.FirstName(),
                SobreNome = faker.Name.LastName(),
                Perfil = "Admin"
            };

            var command = new AuthenticateUserCommand
            {
                email = usuario.Email,
                password = commandPasswordErrado
            };

            _repositoryMock.Setup(r => r.Selecionar(
                It.IsAny<System.Linq.Expressions.Expression<System.Func<UsuarioEntity, bool>>>(),
                It.IsAny<CancellationToken>(),
                It.IsAny<string>()))
                .ReturnsAsync(usuario);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal("Login ou senha inválidos", result.TextResponse);
            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
            Assert.Null(string.IsNullOrEmpty(result.Token) ? null : result.Token);
        }

        [Fact]
        public async Task LogadoComSucesso()
        {
            // Arrange
            var password = faker.Internet.Password();
            var email = faker.Internet.Email();

            var usuario = new UsuarioEntity
            {
                Email = email,
                Senha = HashPassword.StringToHash(password),
                PrimeiroNome = faker.Name.FirstName(),
                SobreNome = faker.Name.LastName(),
                Perfil = "Admin"
            };

            var command = new AuthenticateUserCommand
            {
                email = email,
                password = password
            };

            var expectedToken = faker.Random.AlphaNumeric(30);

            _repositoryMock.Setup(r => r.Selecionar(
                It.IsAny<System.Linq.Expressions.Expression<System.Func<UsuarioEntity, bool>>>(),
                It.IsAny<CancellationToken>(),
                It.IsAny<string>()))
                .ReturnsAsync(usuario);

            _jwtManagerMock.Setup(j => j.GenerateToken(It.IsAny<User>())).Returns(expectedToken);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal("Login realizado com sucesso!", result.TextResponse);
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Equal(expectedToken, result.Token);
        }
    }
}
