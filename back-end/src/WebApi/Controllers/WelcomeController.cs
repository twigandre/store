using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WelcomeController : ControllerBase
    {

        public WelcomeController()
        {                
        }

        [HttpGet]
        public string Get()
        {
            return "Olá Mundo!";
        }
    }
}
