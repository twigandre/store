using Store.App.Crosscutting.Commom.ViewModel.Core.Categoria;
using Store.App.Crosscutting.Commom.ViewModel.Core.Produto;
using Store.App.Infrastructure.Database.DbRepository.Produto;

namespace Store.App.Core.Application.Produto.Selecionar
{
    public class SelecionarProdutoHandler
    {
        IProdutoRepository _repository;
        public SelecionarProdutoHandler(IProdutoRepository repository)
        {
            _repository = repository;
        }

        public async Task<SelecionarProdutoResponse?> Handle(SelecionarProdutoCommand request, CancellationToken cancellationToken)
        {
            var resultado = await _repository.Selecionar(x => x.Id == request.Id, "Categoria,Carro.CarroProduto", cancellationToken);

            if (resultado is null)            
                return null;            

            return new SelecionarProdutoResponse
            {
                Categoria = new CategoriaVM
                {
                    Nome = resultado.Categoria.Nome,
                    Id = resultado.Categoria.Id
                },
                CategoriaId = resultado.CategoriaId,
                Descricao = resultado.Descricao,
                Nome = resultado.Nome,
                Id = resultado.Id,
                PrecoUnitario = resultado.PrecoUnitario,
                Imagem = FtpProduto.DownloadArquivo(resultado.Id),
                Avaliacao = new AvaliacaoVM
                {
                    Nota = resultado.NotaProduto,
                    NumeroVendas = resultado.QuantidadeVendas
                }
            };
        }
    }
}
