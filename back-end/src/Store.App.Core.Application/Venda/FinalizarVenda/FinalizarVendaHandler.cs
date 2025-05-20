using MediatR;
using Store.App.Core.Domain.Entitites;
using Store.App.Core.Domain.Repositories;
using Store.App.Core.Domain.Repositories.Carro;
using Store.App.Core.Domain.Repositories.Venda;
using Store.App.Crosscutting.Commom.ViewModel;
using Store.App.Infrastrucutre.Repositories.Venda;

namespace Store.App.Core.Application.Venda.FinalizarVenda
{
    public class FinalizarVendaHandler : IRequestHandler<FinalizarVendaCommand, FinalizarVendaResponse>
    {
        ICarroRepository _carroRepository;
        IVendaRepository _vendaRepository; 
        IUsuarioRepository _usuarioRepository;
        IFilialRepository _filialRepository;
        IProdutoRepository _produtoRepository;

        public FinalizarVendaHandler(ICarroRepository carroRepository, 
                                     IVendaRepository vendaRepository,
                                     IUsuarioRepository usuarioRepository,
                                     IFilialRepository filialRepository,
                                     IProdutoRepository produtoRepository)
        {
            _carroRepository = carroRepository;
            _vendaRepository = vendaRepository;
            _usuarioRepository = usuarioRepository;
            _filialRepository = filialRepository;
            _produtoRepository = produtoRepository;
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

            var usuarioLogado = await _usuarioRepository.UsuarioLogado(cancellationToken);

            if (usuarioLogado.Id != carro.UsuarioId)
            {
                return new FinalizarVendaResponse
                {
                    TextResponse = "Usuário não autorizado a finalizar a compra deste carro.",
                    StatusCode = System.Net.HttpStatusCode.Forbidden
                };
            }

            decimal valorTotalDaCompra = 0;

            List<CalcularDescontoProdutoVM> calcularDescontoProdutoVMs = new();

            var produtos = await _produtoRepository.Listar(x => x.Id == carro.CarroProduto.Select(x => x.ProdutoId).FirstOrDefault(), cancellationToken, string.Empty);

            carro.CarroProduto.ToList().ForEach(_carroProduto => {

                var produto = produtos.FirstOrDefault(x => x.Id == _carroProduto.ProdutoId);

                var valorProdutoCalculado = (_carroProduto.Quantidade * produto.PrecoUnitario);

                int porcentagemDesconto = 0;

                if (_carroProduto.Quantidade >= 4)
                {
                    if (_carroProduto.Quantidade >= 4 && _carroProduto.Quantidade < 10)
                    {
                        valorProdutoCalculado = valorProdutoCalculado * 0.90m; // 10% de desconto
                        porcentagemDesconto = 10;
                    }
                    else
                    {
                        valorProdutoCalculado = valorProdutoCalculado * 0.80m; // 20% de desconto
                        porcentagemDesconto = 20;
                    }
                }

                valorTotalDaCompra += valorProdutoCalculado;

                calcularDescontoProdutoVMs.Add(new CalcularDescontoProdutoVM
                {
                    IdProduto = _carroProduto.ProdutoId,
                    ValorFinal = valorProdutoCalculado,
                    Quantidade = _carroProduto.Quantidade,
                    PorcentagemDesconto = porcentagemDesconto,
                    ValorTotal = valorProdutoCalculado * _carroProduto.Quantidade
                });
            });

            var numeroVenda = (await _vendaRepository.CountAsync<VendaEntity>(x => x.Id > 0) + 1).ToString();

            var idFilial = _filialRepository.Selecionar(x => x.Id == usuarioLogado.FilialId, cancellationToken, string.Empty).Result.Id;

            VendaEntity venda = new VendaEntity
            {
                UsuarioId = usuarioLogado.Id,
                DataHoraVenda = DateTime.Now,
                FilialId = idFilial,
                ValorTotal = valorTotalDaCompra,
                CarroId = carro.Id,
                Id = 0,
                IsCancelada = false,
                NumeroVenda = numeroVenda
            };

            venda.ItensVenda = carro.CarroProduto.Select(_carroProduto => new VendaItensEntity
            {
                ProdutoId = _carroProduto.ProdutoId,
                Quantidade = _carroProduto.Quantidade,
                Id = 0,
                PrecoUnitario = calcularDescontoProdutoVMs.FirstOrDefault(x => x.IdProduto == _carroProduto.ProdutoId).ValorFinal,
                Desconto = calcularDescontoProdutoVMs.FirstOrDefault(x => x.IdProduto == _carroProduto.ProdutoId).PorcentagemDesconto,
                ValorTotal = calcularDescontoProdutoVMs.FirstOrDefault(x => x.IdProduto == _carroProduto.ProdutoId).ValorTotal,
                VendaId = venda.Id
            })
            .ToList();

            _vendaRepository.Salvar(venda);
            await _vendaRepository.SaveChangesAsync(cancellationToken);

            return new FinalizarVendaResponse { StatusCode = System.Net.HttpStatusCode.OK, TextResponse = "Venda Realizada Com Sucesso!" };
        }
    }
}
