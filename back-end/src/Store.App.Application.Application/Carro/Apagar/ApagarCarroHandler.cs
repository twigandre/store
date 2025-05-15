using MediatR;
using Store.App.Infrastructure.Database.DbRepository.Carro;

namespace Store.App.Core.Application.Carro.Apagar
{
    public class ApagarCarroHandler : IRequestHandler<ApagarCarroCommand, ApagarCarroResult>
    {
        ICarroRepository _carroRepository;
        public ApagarCarroHandler(ICarroRepository carroRepository)
        {
            _carroRepository = carroRepository;
        }
        public async Task<ApagarCarroResult> Handle(ApagarCarroCommand request, CancellationToken cancellationToken)
        {
            var carro = await _carroRepository.Selecionar(x => x.Id == request.Id, cancellationToken);

            if (carro == null)
            {
                return new ApagarCarroResult
                {
                    StatusCode = System.Net.HttpStatusCode.NotFound,
                    TextResponse = "Carro não encontrado"
                };
            }

            if(carro.Status == "compra_realizada")
            {
                return new ApagarCarroResult
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    TextResponse = "Carro não pode ser apagado, pois a compra já foi realizada!"
                };
            }

            _carroRepository.Apagar(carro);
            await _carroRepository.Context.SaveChangesAsync(cancellationToken);

            return new ApagarCarroResult
            {
                TextResponse = "Carro apagado com sucesso!"
            };
        }
    }
}
