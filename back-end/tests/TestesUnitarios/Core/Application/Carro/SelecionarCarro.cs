using Bogus;
using Moq;
using Store.App.Core.Application.Carro.Selecionar;
using Store.App.Core.Domain.Entitites;
using Store.App.Core.Domain.Enum;
using Store.App.Core.Domain.Repositories.Carro;
using System.Linq.Expressions;

namespace TestesUnitarios.Core.Application
{
    public class SelecionarCarro
    {
        private readonly Faker _faker = new Faker("pt_BR");

        [Fact]
        public async Task CarroNaoEncontrado()
        {
            // Arrange
            var repositoryMock = new Mock<ICarroRepository>();

            repositoryMock
            .Setup(r => r.Selecionar(It.IsAny<Expression<Func<CarroEntity, bool>>>(), CancellationToken.None, string.Empty))
            .ReturnsAsync((CarroEntity)null);

            var handler = new SelecionarCarroHandler(repositoryMock.Object);
            var command = new SelecionarCarroCommand { Id = _faker.Random.Int(1, 10) };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task CarroEncontrado()
        {
            var carroId = _faker.Random.Int(1, 10);
            var usuarioId = _faker.Random.Int(1, 10);
            var produtoId = _faker.Random.Int(1, 10);
            var carroProdutoId = _faker.Random.Int(1, 10);
            var dataCriacao = _faker.Date.Past();

            var usuario = new UsuarioEntity
            {
                Id = usuarioId,
                PrimeiroNome = _faker.Name.FirstName(),
                SobreNome = _faker.Name.LastName()
            };

            var carroProduto = new List<CarroProdutoEntity>
            {
                new CarroProdutoEntity
                {
                    Id = carroProdutoId,
                    ProdutoId = produtoId,
                    CarroId = carroId,
                    DataHoraUltimaAlteracao = dataCriacao,
                    Quantidade = _faker.Random.Int(1, 10)
                }
            };

            var carroEntity = new CarroEntity
            {
                Id = carroId,
                UsuarioId = usuarioId,
                DataCriacao = dataCriacao,
                Status = _faker.Random.ArrayElement([StatusCompraCarro.CANCELADO, StatusCompraCarro.COMPRA_REALIZADA, StatusCompraCarro.EM_ANDAMENTO]),
                Usuario = usuario,
                CarroProduto = carroProduto
            };

            var repositoryMock = new Mock<ICarroRepository>();

            // Update the problematic line to use an Expression<Func<>> instead of Func<>
            repositoryMock
                .Setup(r => r.Selecionar(It.IsAny<Expression<Func<CarroEntity, bool>>>(), It.IsAny<CancellationToken>(), It.IsAny<string>()))
                .ReturnsAsync(carroEntity);

            var handler = new SelecionarCarroHandler(repositoryMock.Object);
            var command = new SelecionarCarroCommand { Id = carroId };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(carroId, result.Id);
            Assert.Equal(usuarioId, result.UsuarioId);
            Assert.Equal(dataCriacao, result.DataCriacao);
            Assert.Equal(carroEntity.Status, result.Status);

            Assert.NotNull(result.Comprador);
            Assert.Equal(usuario.Id, result.Comprador.Id);
            Assert.Equal(usuario.PrimeiroNome, result.Comprador.PrimeiroNome);
            Assert.Equal(usuario.SobreNome, result.Comprador.SobreNome);

            Assert.NotNull(result.CarroProduto);
            Assert.Single(result.CarroProduto);

            var produto = result.CarroProduto.First();
            Assert.Equal(carroProdutoId, produto.Id);
            Assert.Equal(produtoId, produto.ProdutoId);
            Assert.Equal(carroId, produto.CarroId);
            Assert.Equal(dataCriacao, produto.DataHoraUltimaAlteracao);
            Assert.Equal(carroProduto.First().Quantidade, produto.Quantidade);
        }

    }
}
