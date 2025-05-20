using Bogus;
using Moq;
using Store.App.Core.Application.Carro.Listar;
using Store.App.Core.Domain.Repositories.Carro;
using Store.App.Core.Domain.Repositories;
using Store.App.Crosscutting.Commom.ViewModel.Core.Carro.Listar;
using Store.App.Crosscutting.Commom.ViewModel.Pagination;
using System.Net;
using Store.App.Core.Domain.Entitites;

namespace TestesUnitarios.Core.Application
{
    public class ListarCarro
    {
        private readonly Faker _faker;
        private readonly Mock<ICarroRepository> _carroRepositoryMock;
        private readonly Mock<IProdutoRepository> _produtoRepositoryMock;
        private readonly ListarCarroHandler _handler;

        public ListarCarro()
        {
            _faker = new Faker("pt_BR");
            _carroRepositoryMock = new Mock<ICarroRepository>();
            _produtoRepositoryMock = new Mock<IProdutoRepository>();
            _handler = new ListarCarroHandler(_carroRepositoryMock.Object, _produtoRepositoryMock.Object);
        }

        [Fact]
        public async Task ProdutoInformadoNaoExiste()
        {
            var command = new ListarCarroCommand();

            command.request = new ListarCarroRequest { IdProduto = _faker.Random.Int(1, 10) };

            _produtoRepositoryMock
            .Setup(r => r.Existe(x => x.Id == command.request.IdProduto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
            Assert.Equal("O Produto informado não existe!", result.TextResponse);
        }

        [Fact]
        public async Task ListagemVazia()
        {
            var command = new ListarCarroCommand();

            command.request = new ListarCarroRequest();

            _carroRepositoryMock
            .Setup(r => r.ListarPaginado(command.request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PagedItems<CarroEntity>
            {
                Total = 0,
                Items = new List<CarroEntity>()
            });

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.NotNull(result.data);
            Assert.Equal(0, result.data.Total);
            Assert.Empty(result.data.Items);
        }

        [Fact]
        public async Task RegistroEncontrado()
        {
            var usuario = new UsuarioEntity
            {
                Id = _faker.Random.Int(1, 10),
                PrimeiroNome = _faker.Name.FirstName(),
                SobreNome = _faker.Name.LastName()
            };

            var carroProduto = new List<CarroProdutoEntity>
            {
                new CarroProdutoEntity
                {
                    Id = _faker.Random.Int(1, 10),
                    ProdutoId = _faker.Random.Int(1, 10),
                    CarroId = 1,
                    DataHoraUltimaAlteracao = _faker.Date.Recent(),
                    Quantidade = _faker.Random.Int(1, 10)
                }
            };

            var carros = new List<CarroEntity>
            {
                new CarroEntity
                {
                    Id = 1,
                    UsuarioId = usuario.Id,
                    DataCriacao = _faker.Date.Past(),
                    Status = "Ativo",
                    Usuario = usuario,
                    CarroProduto = carroProduto
                }
            };

            _carroRepositoryMock
            .Setup(r => r.ListarPaginado(It.IsAny<ListarCarroRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PagedItems<CarroEntity>
            {
                Total = carros.Count,
                Items = carros
            });

            var command = new ListarCarroCommand
            {
                request = new ListarCarroRequest()
            };

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.NotNull(result.data);
            Assert.Equal(carros.Count, result.data.Total);
            Assert.Single(result.data.Items);

            var carroResult = result.data.Items.First();
            Assert.Equal(carros.First().Id, carroResult.Id);
            Assert.Equal(usuario.Id, carroResult.UsuarioId);
            Assert.Equal(usuario.PrimeiroNome, carroResult.Comprador.PrimeiroNome);
            Assert.Single(carroResult.CarroProduto);
            Assert.Equal(carroProduto.First().Id, carroResult.CarroProduto.First().Id);
        }

        [Fact]
        public async Task ExecutarListagemRecebendoNullPorParametro()
        {
            _carroRepositoryMock
            .Setup(r => r.ListarPaginado(It.IsAny<ListarCarroRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PagedItems<CarroEntity>
            {
                Total = 0,
                Items = new List<CarroEntity>()
            });

            var command = new ListarCarroCommand { request = null };

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.NotNull(result.data);
            Assert.Equal(0, result.data.Total);
        }
    }
}
