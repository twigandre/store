using Store.App.Infrastructure.Database.DbRepository.ProdutoCategoria;
using Store.App.Infrastructure.Database.Entities;

namespace Store.App.Core.Application.Services.Produto.ProdutoCategoria
{
    public class ProdutoCategoria : IProdutoCategoria
    {
        IProdutoCategoriaRepository _repository;
        public ProdutoCategoria(IProdutoCategoriaRepository repository)
        {
            _repository = repository;
        }

        public List<ProdutoCategoriaEntity> GetAll()
        {
            var result = _repository.Listar();

            return result;
        }
    }
}
