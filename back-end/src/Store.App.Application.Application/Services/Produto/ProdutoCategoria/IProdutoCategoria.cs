using Store.App.Infrastructure.Database.Entities;

namespace Store.App.Core.Application.Services.Produto.ProdutoCategoria
{
    public interface IProdutoCategoria
    {
        List<ProdutoCategoriaEntity> GetAll();
    }
}
