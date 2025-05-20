using Bogus;
using Moq;
using Store.App.Core.Application.Produto.Listar;
using Store.App.Core.Domain.Entitites;
using Store.App.Core.Domain.Repositories;
using Store.App.Crosscutting.Commom.ViewModel;
using Store.App.Crosscutting.Commom.ViewModel.Core.Categoria;
using Store.App.Crosscutting.Commom.ViewModel.Core.Listar;
using Store.App.Crosscutting.Commom.ViewModel.Core.Produto;
using Store.App.Crosscutting.Commom.ViewModel.Core.Produto.Listar;
using Store.App.Crosscutting.Commom.ViewModel.Pagination;

namespace TestesUnitarios.Core.Application.Produto
{
    public class ListarProduto
    {
        private readonly Mock<IProdutoRepository> _repositoryMock;
        private readonly ListarProdutoHandler _handler;
        private readonly Faker _faker;

        public ListarProduto()
        {
            _repositoryMock = new Mock<IProdutoRepository>();
            _handler = new ListarProdutoHandler(_repositoryMock.Object);
            _faker = new Faker("pt_BR");
        }

        [Fact]
        public async Task ListaVazia_ProdutoNaoEncontrado()
        {
            // Arrange
            var command = new ListarProdutoCommand { request = new ListarProdutoRequest() };

            _repositoryMock
            .Setup(r => r.ListarUsuariosPaginado(It.IsAny<ListarProdutoRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PagedItems<ProdutoEntity> { 
                            Total = 0, 
                            Items = new List<ProdutoEntity>() 
                          }
            );

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.data);
            Assert.Equal(0, result.data.Total);
            Assert.Null(result.data.Items);
        }

        [Fact]
        public async Task ListaVazia_RecebeuNullPorParametro()
        {
            // Arrange
            var command = new ListarProdutoCommand { request = null };

            _repositoryMock
            .Setup(r => r.ListarUsuariosPaginado(It.IsAny<ListarProdutoRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PagedItems<ProdutoEntity>
            {
                Total = 0,
                Items = new List<ProdutoEntity>()
            });

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            _repositoryMock.Verify(r => r.ListarUsuariosPaginado(It.IsAny<ListarProdutoRequest>(), It.IsAny<CancellationToken>()), Times.Once);
            Assert.Equal(0, result.data.Total);
        }

        /*
        [Fact]
        public async Task RegistroEncontrado()
        {
            // Arrange
            var produtoFaker = new Faker<ListarProdutoResult>()
            .CustomInstantiator(f => new ListarProdutoResult
            {
                Categoria = new CategoriaVM
                {
                    Id = f.Random.Int(1, 10),
                    Nome = f.Commerce.Categories(1).First()
                },
                CategoriaId = f.Random.Int(1, 10),
                Descricao = f.Commerce.ProductDescription(),
                Nome = f.Commerce.ProductName(),
                Id = f.Random.Int(1, 10),
                PrecoUnitario = f.Random.Decimal(1, 1000),
                Avaliacao = new AvaliacaoVM
                {
                    Nota = f.Random.Int(0, 5),
                    NumeroVendas = f.Random.Int(1, 1000)
                },
                Imagem = new ArquivoVM
                {
                    Nome = "imagem-fake.jpg"
                }
            });

            var produtos = produtoFaker.Generate(3);

            _repositoryMock
            .Setup(r => r.ListarUsuariosPaginado(It.IsAny<ListarProdutoRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PagedItems<ProdutoEntity>
            {
                Total = produtos.Count,
                Items = produtos.Select(x => new ProdutoEntity
                {
                    Categoria = new ProdutoCategoriaEntity
                    {
                        Id = x.Categoria.Id,
                        Nome = x.Categoria.Nome
                    },
                    CategoriaId = x.CategoriaId,
                    Descricao = x.Descricao,
                    Nome = x.Nome,
                    Id = (int)x.Id,
                    PrecoUnitario = x.PrecoUnitario,
                    Avaliacao = new AvaliacaoVM
                    {
                        Nota = f.Random.Int(0, 5),
                        NumeroVendas = f.Random.Int(1, 1000)
                    },
                    Imagem = new ArquivoVM
                    {
                        Nome = "imagem-fake.jpg"
                    }
                }).ToList()
            });

            Simulando o retorno da imagem
            FtpProdutoShim.DownloadArquivo = id => produtos.First().Imagem.Nome;

            var command = new ListarProdutoCommand { request = new ListarProdutoRequest() };

            Act
           var result = await _handler.Handle(command, CancellationToken.None);

            Assert
            Assert.NotNull(result);
            Assert.NotNull(result.data);
            Assert.Equal(produtos.Count, result.data.Total);
            Assert.Single(result.data.Items);

            var produtoEsperado = produtos.First();
            var produtoRetornado = result.data.Items.First();

            Assert.Equal(produtoEsperado.Categoria.Nome, produtoRetornado.Categoria.Nome);
            Assert.Equal(produtoEsperado.Descricao, produtoRetornado.Descricao);
            Assert.Equal(produtoEsperado.Nome, produtoRetornado.Nome);
            Assert.Equal(produtoEsperado.PrecoUnitario, produtoRetornado.PrecoUnitario);
            Assert.Equal("imagem-fake.jpg", produtoRetornado.Imagem.Nome);
            Assert.Equal(produtoEsperado.Avaliacao.Nota, produtoRetornado.Avaliacao.Nota);
            Assert.Equal(produtoEsperado.Avaliacao.NumeroVendas, produtoRetornado.Avaliacao.NumeroVendas);
        }
        */

        public static class FtpProdutoShim
        {
            public static Func<int, string> DownloadArquivo = id => null;
        }

        public static class FtpProduto
        {
            public static string DownloadArquivo(int id) => FtpProdutoShim.DownloadArquivo(id);
        }
    }
}
