using MediatR;
using Store.App.Core.Domain.Repositories;
using Store.App.Core.Domain.Repositories.Carro;
using Store.App.Crosscutting.Commom.ViewModel.Core.Carro;
using Store.App.Crosscutting.Commom.ViewModel.Core.Carro.Listar;
using Store.App.Crosscutting.Commom.ViewModel.Core.Usuario;
using Store.App.Crosscutting.Commom.ViewModel.Pagination;

namespace Store.App.Core.Application.Carro.Listar
{
    public class ListarCarroHandler : IRequestHandler<ListarCarroCommand, ListarCarroResponse>
    {
        ICarroRepository _repository;
        IProdutoRepository _produtoRepository;
        public ListarCarroHandler(ICarroRepository repository, IProdutoRepository produtoRepository)
        {
            _repository = repository;
            _produtoRepository = produtoRepository; 
        }

        public async Task<ListarCarroResponse> Handle(ListarCarroCommand obj, CancellationToken cancellationToken)
        {
            var filter = obj.request is null ? new ListarCarroRequest() : obj.request;

            if(filter?.IdProduto > 0)
            {
                bool produtoExisteNaBase = await _produtoRepository.Existe(x => x.Id == filter.IdProduto, cancellationToken);

                if (!produtoExisteNaBase)
                {
                    return new ListarCarroResponse
                    {
                        StatusCode = System.Net.HttpStatusCode.NotFound,
                        TextResponse = "O Produto informado não existe!"
                    };
                }   
            }

            var resultadoPaginado = await _repository.ListarPaginado(filter, cancellationToken);

            var retornoMetodo = new ListarCarroResponse 
            {
                data = new PagedItems<ListarCarroResult>(),
                StatusCode = System.Net.HttpStatusCode.OK
            };

            if (resultadoPaginado.Total == 0)
                return retornoMetodo;

            retornoMetodo.data.Total = resultadoPaginado.Total;
            retornoMetodo.data.Items = resultadoPaginado.Items.Select(carro => new ListarCarroResult
            {
                Id = carro.Id,
                UsuarioId = carro.UsuarioId,
                DataCriacao = carro.DataCriacao,
                Status = carro.Status,
                Comprador = new UsuarioVM
                {
                    Id = carro.Usuario.Id,
                    PrimeiroNome = carro.Usuario.PrimeiroNome,
                    SobreNome = carro.Usuario.SobreNome
                },
                CarroProduto = carro.CarroProduto.Select(x => new CarroProdutoVM()
                {
                    Id = x.Id,
                    ProdutoId = x.ProdutoId,
                    CarroId = x.CarroId,
                    DataHoraUltimaAlteracao = x.DataHoraUltimaAlteracao,
                    Quantidade = x.Quantidade
                })
                .ToList()
            })
            .ToList();

            return retornoMetodo;
        }
    }
}
