using MediatR;
using Microsoft.AspNetCore.Mvc;
using Store.App.Core.Application.Auth;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        IMediator _mediator;
        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<AuthenticateUserResult>> Login ([FromBody] AuthenticateUserCommand obj, CancellationToken cancellationToken)
        {
            AuthenticateUserResult result = await _mediator.Send(obj, cancellationToken);

            return (result.StatusCode == System.Net.HttpStatusCode.OK) ? Ok(result) : NotFound(result);
        }
    }
}
