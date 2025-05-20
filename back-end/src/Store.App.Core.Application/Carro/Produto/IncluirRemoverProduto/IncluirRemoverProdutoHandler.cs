using MediatR;
using Store.App.Crosscutting.Commom.ViewModel.Core.Carro;
using Store.App.Crosscutting.Commom.ViewModel;
using Store.App.Core.Domain.Repositories;
using Store.App.Core.Domain.Entitites;
using Store.App.Core.Domain.Repositories.Carro;

namespace Store.App.Core.Application.Carro.Produto.IncluirRemoverProduto
{
    public class IncluirRemoverProdutoHandler : IRequestHandler<IncluirRemoverProdutoCommand, IncluirRemoverProdutoResult>
    {
        ICarroRepository _repositoryCarro;
        ICarroProdutoRepository _carroProdutoRepository;
        IUsuarioRepository _usuarioRepository;
        public IncluirRemoverProdutoHandler(ICarroRepository repositoryCarro, 
                                            ICarroProdutoRepository carroProdutoRepository, 
                                            IUsuarioRepository usuarioRepository)
        {
            _repositoryCarro = repositoryCarro;
            _carroProdutoRepository = carroProdutoRepository;
            _usuarioRepository = usuarioRepository;
        }

        public async Task<IncluirRemoverProdutoResult> Handle(IncluirRemoverProdutoCommand request, CancellationToken cancellationToken)
        {
            RequestResponseVM response = new ();

            var produto = new ManterCarroProdutoVM { 
                IdProduto = request.IdProduto
            };

            CarroEntity? carro = await _repositoryCarro.RecuperarCarroAtivoDoUsuarioLogado(cancellationToken);

            if (request.tipoOperacao == "incluir")
            {
                if (carro is null)
                {
                    UsuarioEntity usuarioLogadoFromDb = await _usuarioRepository.UsuarioLogado(cancellationToken);

                    carro = await _repositoryCarro.CriarCarroParaUsuario(usuarioLogadoFromDb.Id, cancellationToken);
                }

                produto.IdCarro = carro.Id;

                response = await _carroProdutoRepository.IncluirProdutoNoCarro(produto, cancellationToken);
            }
            else
            {
                if (carro is null)
                {
                    return new IncluirRemoverProdutoResult
                    {
                        TextResponse = "usuário logado nao possui um carro vinculado.",
                        StatusCode = System.Net.HttpStatusCode.BadRequest
                    };
                }

                produto.IdCarro = carro.Id;

                response = await _carroProdutoRepository.RemoverProdutoDoCarro(produto, cancellationToken);
            }

            return new IncluirRemoverProdutoResult
            {
                StatusCode = response.StatusCode,
                TextResponse = response.TextResponse
            };
        }
    }
}
