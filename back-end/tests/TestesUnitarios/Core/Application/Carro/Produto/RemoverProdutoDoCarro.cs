using Bogus;
using Moq;
using Store.App.Core.Application.Carro.Produto.RemoverProduto;
using Store.App.Core.Domain.Entitites;
using Store.App.Core.Domain.Repositories.Carro;
using Store.App.Core.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TestesUnitarios.Core.Application.Carro.Produto
{
    public class RemoverProdutoDoCarro
    {
        private readonly Mock<ICarroRepository> _carroRepositoryMock;
        private readonly Mock<ICarroProdutoRepository> _carroProdutoRepositoryMock;
        private readonly RemoverProdutoHandler _handler;
        private readonly Faker _faker;

        public RemoverProdutoDoCarro()
        {
            _faker = new Faker("pt_BR");
            _carroRepositoryMock = new Mock<ICarroRepository>();
            _carroProdutoRepositoryMock = new Mock<ICarroProdutoRepository>();
            _handler = new RemoverProdutoHandler(_carroRepositoryMock.Object, _carroProdutoRepositoryMock.Object);
        }

        [Fact]
        public async Task NaoRemoverProduto_CarroUsuarioNaoEncontrado()
        {
            // Arrange
            _carroRepositoryMock
                .Setup(r => r.RecuperarCarroAtivoDoUsuarioLogado(It.IsAny<CancellationToken>()))
                .ReturnsAsync((CarroEntity?)null);

            var command = new RemoverProdutoCommand
            {
                IdProduto = _faker.Random.Int(1, 1000)
            };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal("Falha ao recuperar carro do usuário logado.", result.TextResponse);
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        }

        [Fact]
        public async Task NaoRemoverProduto_ProdutoNaoVinculadoAoCarro()
        {
            // Arrange
            var carro = new CarroEntity { Id = _faker.Random.Int(1000, 2000) };

            _carroRepositoryMock
                .Setup(r => r.RecuperarCarroAtivoDoUsuarioLogado(It.IsAny<CancellationToken>()))
                .ReturnsAsync(carro);

            _carroProdutoRepositoryMock
                .Setup(r => r.Selecionar(It.IsAny<System.Linq.Expressions.Expression<System.Func<CarroProdutoEntity, bool>>>(),
                                         It.IsAny<CancellationToken>(),
                                         It.IsAny<string>()))
                .ReturnsAsync((CarroProdutoEntity)null);

            var command = new RemoverProdutoCommand
            {
                IdProduto = _faker.Random.Int(1, 1000)
            };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal("Produto não está vinculado ao carro.", result.TextResponse);
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        }

        [Fact]
        public async Task RemoverProdutoComSucesso()
        {
            // Arrange
            var carroId = _faker.Random.Int(1000, 2000);
            var produtoId = _faker.Random.Int(1, 1000);
            var carro = new CarroEntity { Id = carroId };
            var carroProduto = new CarroProdutoEntity { CarroId = carroId, ProdutoId = produtoId };

            _carroRepositoryMock
                .Setup(r => r.RecuperarCarroAtivoDoUsuarioLogado(It.IsAny<CancellationToken>()))
                .ReturnsAsync(carro);

            _carroProdutoRepositoryMock
                .Setup(r => r.Selecionar(It.IsAny<System.Linq.Expressions.Expression<System.Func<CarroProdutoEntity, bool>>>(),
                                         It.IsAny<CancellationToken>(),
                                         It.IsAny<string>()))
                .ReturnsAsync(carroProduto);

            _carroProdutoRepositoryMock
                .Setup(r => r.Apagar(It.IsAny<CarroProdutoEntity>()));

            _carroProdutoRepositoryMock
                .Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var command = new RemoverProdutoCommand { IdProduto = produtoId };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            _carroProdutoRepositoryMock.Verify(r => r.Apagar(carroProduto), Times.Once);
            _carroProdutoRepositoryMock.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);

            Assert.Equal("Produto removido do carro com sucesso.", result.TextResponse);
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        }
    }
}
