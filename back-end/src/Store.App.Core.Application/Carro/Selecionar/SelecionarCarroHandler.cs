using MediatR;
using Store.App.Crosscutting.Commom.ViewModel.Core.Usuario;
using Store.App.Crosscutting.Commom.ViewModel.Core.Carro;
using Store.App.Core.Domain.Repositories;
using Store.App.Core.Domain.Entitites;
using Store.App.Core.Domain.Repositories.Carro;

namespace Store.App.Core.Application.Carro.Selecionar
{
    public class SelecionarCarroHandler : IRequestHandler<SelecionarCarroCommand, SelecionarCarroResult>
    {
        ICarroRepository _repository;
        public SelecionarCarroHandler(ICarroRepository repository)
        {
            _repository = repository;
        }

        public async Task<SelecionarCarroResult?> Handle(SelecionarCarroCommand request, CancellationToken cancellationToken)
        {
            CarroEntity carro = await _repository.Selecionar(x => x.Id == request.Id, cancellationToken, "Usuario,CarroProduto");

            if(carro is null)            
                return null;            

            SelecionarCarroResult result = new SelecionarCarroResult()
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
            };

            return result;
        }
    }
}
