using Microsoft.AspNetCore.Mvc;
using Store.App.Core.Application.Usuario;
using Store.App.Crosscutting.Commom.ViewModel;
using Store.App.Crosscutting.Commom.ViewModel.Login;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        IUsuario _service;
        public AuthController(IUsuario service)
        {
            _service = service;
        }

        [HttpPost("[action]")]
        public IActionResult Login([FromBody] LoginRequest obj)
        {
            RequestResponse result = _service.Logar(obj);

            if (result.StatusCode == System.Net.HttpStatusCode.OK)
                return Ok(result.TextResponse);
            else
                return NotFound(result.TextResponse);
        }
    }
}
