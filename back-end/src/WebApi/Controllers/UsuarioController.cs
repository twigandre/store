using MediatR;
using Microsoft.AspNetCore.Mvc;
using Store.App.Core.Application.Usuario.Apagar;
using Store.App.Core.Application.Usuario.Listar;
using Store.App.Core.Application.Usuario.Salvar;
using Store.App.Core.Application.Usuario.Selecionar;
using Store.App.Crosscutting.Commom.ViewModel;
using Store.App.Crosscutting.Commom.ViewModel.Core.Application.Usuario.Salvar;
using System.Net;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsuarioController : ControllerBase
    {
        IMediator _mediator;
        public UsuarioController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ListarUsuarioResponse> Get([FromQuery] ListarUsuarioCommand request, CancellationToken cancellationToken) =>
            await _mediator.Send(request, cancellationToken);

        [HttpGet("{id:int}")]
        public async Task<ActionResult<SelecionarUsuarioResponse>> Get(int id, CancellationToken cancellationToken)
        {
            var command = new SelecionarUsuarioCommand { Id = id };
            var result = await _mediator.Send(command, cancellationToken);
            return result is null ? NotFound("Usuário não encontrado") : Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<SalvarUsuarioResponse>> Post([FromBody] SalvarUsuarioCommand request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(request, cancellationToken);

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var command = new ApagarUsuarioCommand { Id = id };
            var result = await _mediator.Send(command, cancellationToken);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}