using System.Net;
using Bogus;
using Moq;
using Store.App.Core.Application.Carro.Apagar;
using Store.App.Core.Domain.Entitites;
using Store.App.Core.Domain.Enum;
using Store.App.Core.Domain.Repositories.Carro;

namespace TestesUnitarios.Core.Application.Carro
{
    public class ApagarCarro
    {
        private readonly Faker _faker;
        private readonly Mock<ICarroRepository> _carroRepositoryMock;
        private readonly ApagarCarroHandler _handler;

        public ApagarCarro()
        {
            _faker = new Faker("pt_BR");
            _carroRepositoryMock = new Mock<ICarroRepository>();
            _handler = new ApagarCarroHandler(_carroRepositoryMock.Object);
        }

        [Fact]
        public async Task CarroNaoEncontradoNaBase()
        {
            // Arrange
            var command = new ApagarCarroCommand { Id = _faker.Random.Int(1, 10) };

            _carroRepositoryMock
                .Setup(r => r.Selecionar(x => x.Id == command.Id, It.IsAny<CancellationToken>(), It.IsAny<string>()))
                .ReturnsAsync((CarroEntity)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
        }

        [Fact]
        public async Task CompraJaRealizada()
        {
            // Arrange
            int id = _faker.Random.Int(1, 10);

            var carro = new CarroEntity { Id = id, Status = StatusCompraCarro.COMPRA_REALIZADA };
            
            var command = new ApagarCarroCommand { Id = id };
            _carroRepositoryMock
                .Setup(r => r.Selecionar(x => x.Id == command.Id, It.IsAny<CancellationToken>(), It.IsAny<string>()))
                .ReturnsAsync(carro);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        }

        [Fact]
        public async Task CompraNaoFoiRealizada()
        {
            // Arrange
            int id = _faker.Random.Int(1, 1000);

            var carro = new CarroEntity { Id = id, Status = StatusCompraCarro.EM_ANDAMENTO };
            
            var command = new ApagarCarroCommand { Id = id };

            _carroRepositoryMock
                .Setup(r => r.Selecionar(x => x.Id == command.Id, It.IsAny<CancellationToken>(), It.IsAny<string>()))
                .ReturnsAsync(carro);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            _carroRepositoryMock.Verify(r => r.Apagar(carro), Times.Once);
            _carroRepositoryMock.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);

            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        }
    }
}