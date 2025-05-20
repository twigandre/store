using Bogus;
using Moq;
using Store.App.Core.Application.Produto.Apagar;
using Store.App.Core.Domain.Entitites;
using Store.App.Core.Domain.Repositories.Carro;
using Store.App.Core.Domain.Repositories;
using System.Linq.Expressions;
using Store.App.Core.Domain.Enum;
using System.Net;

namespace TestesUnitarios.Core.Application.Produto
{
    public class ApagarProduto
    {
        private readonly Mock<IProdutoRepository> _produtoRepositoryMock;
        private readonly Mock<ICarroRepository> _carroRepositoryMock;
        private readonly ApagarProdutoHandler _handler;
        private readonly Faker _faker;

        private DateTime hoje; 
        private DateTime semanaPassada; 

        public ApagarProduto()
        {
            _produtoRepositoryMock = new Mock<IProdutoRepository>();
            _carroRepositoryMock = new Mock<ICarroRepository>();
            _handler = new ApagarProdutoHandler(_produtoRepositoryMock.Object, _carroRepositoryMock.Object);
            _faker = new Faker("pt_BR");

            hoje = DateTime.Today;
            semanaPassada = hoje.AddDays(-7);
        }

        private ProdutoEntity CriarProdutoComCarros(String status, int produtoId)
        {
            var carros = new List<CarroEntity>
            {
                new CarroEntity
                {
                    Id = _faker.Random.Int(1, 1000),
                    DataCriacao = _faker.Date.Between(semanaPassada, hoje),
                    Status = status,
                    UsuarioId = _faker.Random.Int(1, 10)
                },
                new CarroEntity
                {
                    Id = _faker.Random.Int(1, 1000),
                    DataCriacao = _faker.Date.Between(semanaPassada, hoje),
                    Status = status,
                    UsuarioId = _faker.Random.Int(1, 10)
                }
            };

            return new ProdutoEntity
            {
                Id = produtoId,
                Nome = _faker.Commerce.ProductName(),
                Carro = carros,
                CategoriaId = _faker.Random.Int(1, 10),
                Descricao = _faker.Commerce.ProductName(),
                PrecoUnitario = Convert.ToDecimal(_faker.Commerce.Price()),
                QuantidadeVendas = _faker.Random.Int(1, 1000),
                NotaProduto = _faker.Random.Int(0, 5)
            };
        }

        private ProdutoEntity CriarProdutoSemCarros(int produtoId)
        {
            return new ProdutoEntity
            {
                Id = produtoId,
                Nome = _faker.Commerce.ProductName(),
                Carro = new List<CarroEntity>(),
                CategoriaId = _faker.Random.Int(1, 10),
                Descricao = _faker.Commerce.ProductName(),
                PrecoUnitario = Convert.ToDecimal(_faker.Commerce.Price()),
                QuantidadeVendas = _faker.Random.Int(1, 1000),
                NotaProduto = _faker.Random.Int(0, 5)
            };
        }

        [Fact]
        public async Task NaoApagouProduto_ProdutoNaoEncontrado()
        {
            // Arrange
            var command = new ApagarProdutoCommand
            {
                Id = _faker.Random.Int(1, 9999) // Gera um ID randômico
            };

            _produtoRepositoryMock
                .Setup(r => r.Selecionar(
                    It.IsAny<System.Linq.Expressions.Expression<Func<ProdutoEntity, bool>>>(),
                    It.IsAny<CancellationToken>(),
                    "Carro"))
                .ReturnsAsync((ProdutoEntity)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
            Assert.Equal("Produto não encontrado.", result.TextResponse);
        }

        [Fact]
        public async Task NaoApagouProduto_UmaCompraJaFoiEfetuadaParaOProdutoEscolhido()
        {
            // Arrange
            var produtoId = _faker.Random.Int(1, 10);

            var produtoEntity = new ProdutoEntity
            {
                Id = produtoId,
                Carro = new List<CarroEntity>
                {
                    new CarroEntity
                    {
                        Id = _faker.Random.Int(1, 10),
                        Status = StatusCompraCarro.COMPRA_REALIZADA,
                        DataCriacao = _faker.Date.Between(semanaPassada, hoje),
                        UsuarioId = _faker.Random.Int(1, 10)
                    }
                },
                CategoriaId = _faker.Random.Int(1, 10),
                Descricao = _faker.Commerce.ProductName(),
                PrecoUnitario = Convert.ToDecimal(_faker.Commerce.Price()),
                QuantidadeVendas = _faker.Random.Int(1, 1000),
                NotaProduto = _faker.Random.Int(0, 5)
            };

            _produtoRepositoryMock
            .Setup(r => r.Selecionar(It.IsAny<Expression<System.Func<ProdutoEntity, bool>>>(), It.IsAny<CancellationToken>(), "Carro"))
            .ReturnsAsync(produtoEntity);

            var command = new ApagarProdutoCommand { Id = produtoId };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Equal("Produto não pode ser excluído, pois já teve compra realizada.", result.TextResponse);

            _carroRepositoryMock.Verify(r => r.RemoveRange(It.IsAny<IEnumerable<CarroEntity>>()), Times.Never);
            _produtoRepositoryMock.Verify(r => r.Apagar(It.IsAny<ProdutoEntity>()), Times.Never);
        }

        [Fact]
        public async Task ProdutoPagado_NaoExisteCarroComCompraRealizada()
        {
            // Arrange
            var produtoId = _faker.Random.Int(1, 10);

            var produtoEntity = CriarProdutoComCarros(StatusCompraCarro.EM_ANDAMENTO, produtoId);

            _produtoRepositoryMock
                .Setup(r => r.Selecionar(It.IsAny<Expression<System.Func<ProdutoEntity, bool>>>(), It.IsAny<CancellationToken>(), "Carro"))
                .ReturnsAsync(produtoEntity);

            _carroRepositoryMock.Setup(r => r.RemoveRange(produtoEntity.Carro));
            _carroRepositoryMock.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            _produtoRepositoryMock.Setup(r => r.Apagar(It.IsAny<ProdutoEntity>()));
            _produtoRepositoryMock.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            var command = new ApagarProdutoCommand { Id = produtoId };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Equal("Produto apagado com sucesso!", result.TextResponse);

            _carroRepositoryMock.Verify(r => r.RemoveRange(produtoEntity.Carro), Times.Once);
            _carroRepositoryMock.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            _produtoRepositoryMock.Verify(r => r.Apagar(It.Is<ProdutoEntity>(p => p.Id == produtoId)), Times.Once);
            _produtoRepositoryMock.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task ProdutoPagado_NaoExisteCarro()
        {
            // Arrange
            var produtoId = _faker.Random.Int(1, 10);

            var produtoEntity = CriarProdutoSemCarros(produtoId);

            _produtoRepositoryMock
                .Setup(r => r.Selecionar(It.IsAny<Expression<System.Func<ProdutoEntity, bool>>>(), It.IsAny<CancellationToken>(), "Carro"))
                .ReturnsAsync(produtoEntity);

            _produtoRepositoryMock.Setup(r => r.Apagar(It.IsAny<ProdutoEntity>()));
            _produtoRepositoryMock.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            var command = new ApagarProdutoCommand { Id = produtoId };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Equal("Produto apagado com sucesso!", result.TextResponse);

            _carroRepositoryMock.Verify(r => r.RemoveRange(It.IsAny<IEnumerable<CarroEntity>>()), Times.Never);
            _carroRepositoryMock.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);

            _produtoRepositoryMock.Verify(r => r.Apagar(It.Is<ProdutoEntity>(p => p.Id == produtoId)), Times.Once);
            _produtoRepositoryMock.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
