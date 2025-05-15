using MediatR;
using Store.App.Crosscutting.Commom.Security.JwtManager;
using Store.App.Infrastructure.Database.DbEntities;
using Store.App.Infrastructure.Database.DbRepository.Carro;
using Store.App.Infrastructure.Database.DbRepository.Usuario;
using Store.App.Insfrastructure.Database.DbRepository.Carro.CarroProduto;
using Store.App.Crosscutting.Commom.ViewModel.Core.Carro;
using Store.App.Crosscutting.Commom.ViewModel;

namespace Store.App.Core.Application.Carro.Produto.IncluirRemoverProduto
{
    public class IncluirRemoverProdutoHandler : IRequestHandler<IncluirRemoverProdutoCommand, IncluirRemoverProdutoResult>
    {
        IJwtManager _jwt;
        ICarroRepository _repositoryCarro;
        ICarroProdutoRepository _carroProdutoRepository;
        IUsuarioRepository _usuarioRepository;
        public IncluirRemoverProdutoHandler(IJwtManager jwt, 
                                          ICarroRepository repositoryCarro, 
                                          ICarroProdutoRepository carroProdutoRepository, 
                                          IUsuarioRepository usuarioRepository)
        {
            _jwt = jwt;
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
