using MediatR;
using Store.App.Crosscutting.Commom.ViewModel;
using Store.App.Infrastructure.Database.DbRepository.Carro;
using Store.App.Infrastructure.Database.DbRepository.Venda;

namespace Store.App.Core.Application.Venda.FinalizarVenda
{
    public class FinalizarVendaHandler : IRequestHandler<FinalizarVendaCommand, FinalizarVendaResponse>
    {
        ICarroRepository _carroRepository;
        IVendaRepository _vendaRepository;

        public FinalizarVendaHandler(ICarroRepository carroRepository, IVendaRepository vendaRepository)
        {
            _carroRepository = carroRepository;
            _vendaRepository = vendaRepository;
        }
        public async Task<FinalizarVendaResponse> Handle(FinalizarVendaCommand request, CancellationToken cancellationToken)
        {
            var carro = await _carroRepository.Selecionar(x => x.Id == request.IdCarro && x.Status == "em_andamento", cancellationToken, "CarroProduto");

            if (carro is null)
            {
                return new FinalizarVendaResponse
                {
                    StatusCode = System.Net.HttpStatusCode.NotFound,
                    TextResponse = "Carro não encontrado ou não está em andamento."
                };
            }

            RequestResponseVM result = await _vendaRepository.FinalizarCompra(carro, cancellationToken);

            return new FinalizarVendaResponse();
        }
    }
}
