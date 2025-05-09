﻿using Store.App.Infrastructure.Database.DbEntities;

namespace Store.App.Infrastructure.Database.DbRepository.ProdutoCategoria
{
    public class ProdutoCategoriaRepository : GenericRepository<ProdutoCategoriaEntity>, IProdutoCategoriaRepository
    {
        public ProdutoCategoriaRepository(StoreContext db_dbContext) : base(db_dbContext)
        {
        }
    }
}
