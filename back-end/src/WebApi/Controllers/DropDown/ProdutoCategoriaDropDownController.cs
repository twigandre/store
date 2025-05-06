using Microsoft.AspNetCore.Mvc;
using Store.App.Core.Application.Services.Produto.ProdutoCategoria;
using Store.App.Infrastructure.Database.Entities;

namespace WebApi.Controllers.DropDown
{
    [ApiController]
    [Route("[controller]")]
    public class ProdutoCategoriaDropDownController : ControllerBase
    {
        IProdutoCategoria _service;
        public ProdutoCategoriaDropDownController(IProdutoCategoria service)
        {
            _service = service;
        }

        [HttpGet]
        public List<ProdutoCategoriaEntity> Get() => _service.GetAll();

    }
}
