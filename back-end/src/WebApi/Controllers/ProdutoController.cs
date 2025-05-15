using MediatR;
using Microsoft.AspNetCore.Mvc;
using Store.App.Core.Application.Produto.Apagar;
using Store.App.Core.Application.Produto.Listar;
using Store.App.Core.Application.Produto.Salvar;
using Store.App.Core.Application.Produto.Selecionar;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProdutoController : ControllerBase
    {
        IMediator _mediator;
        public ProdutoController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ListarProdutoResponse> Get([FromQuery] ListarProdutoCommand request, CancellationToken cancellationToken) =>
           await _mediator.Send(request, cancellationToken);

        [HttpGet("{id:int}")]
        public async Task<ActionResult<SelecionarProdutoResponse>> Get(int id, CancellationToken cancellationToken)
        {
            var command = new SelecionarProdutoResponse { Id = id };
            var result = await _mediator.Send(command, cancellationToken);
            return result is null ? NotFound("Produto não encontrado") : Ok(result);
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<SalvarProdutoResponse>> Selecionar([FromBody] SalvarProdutoCommand obj, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(obj, cancellationToken);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var command = new ApagarProdutoCommand { Id = id };
            var result = await _mediator.Send(command, cancellationToken);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}

