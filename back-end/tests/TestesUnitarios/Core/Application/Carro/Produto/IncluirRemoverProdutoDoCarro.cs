using Bogus;
using Moq;
using Store.App.Core.Application.Carro.Produto.IncluirRemoverProduto;
using Store.App.Core.Domain.Entitites;
using Store.App.Core.Domain.Repositories.Carro;
using Store.App.Core.Domain.Repositories;
using Store.App.Crosscutting.Commom.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Store.App.Crosscutting.Commom.ViewModel.Core.Carro;

namespace TestesUnitarios.Core.Application.Carro.Produto
{
    public class IncluirRemoverProdutoDoCarro
    {
        private readonly Mock<ICarroRepository> _carroRepositoryMock;
        private readonly Mock<ICarroProdutoRepository> _carroProdutoRepositoryMock;
        private readonly Mock<IUsuarioRepository> _usuarioRepositoryMock;
        private readonly IncluirRemoverProdutoHandler _handler;
        private readonly Faker _faker;

        public IncluirRemoverProdutoDoCarro()
        {
            _faker = new Faker("pt_BR");
            _carroRepositoryMock = new Mock<ICarroRepository>();
            _carroProdutoRepositoryMock = new Mock<ICarroProdutoRepository>();
            _usuarioRepositoryMock = new Mock<IUsuarioRepository>();

            _handler = new IncluirRemoverProdutoHandler(
                _carroRepositoryMock.Object,
                _carroProdutoRepositoryMock.Object,
                _usuarioRepositoryMock.Object
            );
        }

        [Fact]
        public async Task IncluirProduto_CarroJaExistente()
        {
            // Arrange
            var produtoId = _faker.Random.Int(1, 1000);

            var carroId = _faker.Random.Int(1, 1000);

            var command = new IncluirRemoverProdutoCommand { IdProduto = produtoId, tipoOperacao = "incluir" };

            var carro = new CarroEntity { Id = carroId };

            var response = new RequestResponseVM { StatusCode = HttpStatusCode.OK, TextResponse = "Incluído" };

            _carroRepositoryMock.Setup(r => r.RecuperarCarroAtivoDoUsuarioLogado(It.IsAny<CancellationToken>()))
                .ReturnsAsync(carro);

            _carroProdutoRepositoryMock.Setup(r => r.IncluirProdutoNoCarro(It.IsAny<ManterCarroProdutoVM>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);

            _carroProdutoRepositoryMock.Verify(r =>
                r.IncluirProdutoNoCarro(It.Is<ManterCarroProdutoVM>(p => p.IdProduto == produtoId && p.IdCarro == carroId), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task IncluirProduto_CarroNaoExiste()
        {
            // Arrange
            var produtoId = _faker.Random.Int(1, 1000);

            var usuario = new UsuarioEntity { Id = _faker.Random.Int(1000, 9999) };

            var novoCarro = new CarroEntity { Id = _faker.Random.Int(1000, 9999) };

            var response = new RequestResponseVM { StatusCode = HttpStatusCode.OK, TextResponse = "Incluído" };

            var command = new IncluirRemoverProdutoCommand { IdProduto = produtoId, tipoOperacao = "incluir" };

            _carroRepositoryMock.Setup(r => r.RecuperarCarroAtivoDoUsuarioLogado(It.IsAny<CancellationToken>()))
                .ReturnsAsync((CarroEntity?)null);

            _usuarioRepositoryMock.Setup(r => r.UsuarioLogado(It.IsAny<CancellationToken>()))
                .ReturnsAsync(usuario);

            _carroRepositoryMock.Setup(r => r.CriarCarroParaUsuario(usuario.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(novoCarro);

            _carroProdutoRepositoryMock.Setup(r => r.IncluirProdutoNoCarro(It.IsAny<ManterCarroProdutoVM>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);

            _carroRepositoryMock.Verify(r => r.CriarCarroParaUsuario(usuario.Id, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task RemoverProduto_CarroNaoExiste()
        {
            // Arrange
            var command = new IncluirRemoverProdutoCommand
            {
                IdProduto = _faker.Random.Int(1, 1000),
                tipoOperacao = "remover"
            };

            _carroRepositoryMock.Setup(r => r.RecuperarCarroAtivoDoUsuarioLogado(It.IsAny<CancellationToken>()))
                .ReturnsAsync((CarroEntity?)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);

            Assert.Equal("usuário logado nao possui um carro vinculado.", result.TextResponse);
        }

        [Fact]
        public async Task RemoverProduto_CarroExiste()
        {
            // Arrange
            var produtoId = _faker.Random.Int(1, 1000);

            var carroId = _faker.Random.Int(1000, 9999);
            
            var command = new IncluirRemoverProdutoCommand { IdProduto = produtoId, tipoOperacao = "remover" };
            
            var carro = new CarroEntity { Id = carroId };
            
            var response = new RequestResponseVM { StatusCode = HttpStatusCode.OK, TextResponse = "Removido" };

            _carroRepositoryMock.Setup(r => r.RecuperarCarroAtivoDoUsuarioLogado(It.IsAny<CancellationToken>()))
                .ReturnsAsync(carro);

            _carroProdutoRepositoryMock.Setup(r => r.RemoverProdutoDoCarro(It.IsAny<ManterCarroProdutoVM>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        }
    }
}
