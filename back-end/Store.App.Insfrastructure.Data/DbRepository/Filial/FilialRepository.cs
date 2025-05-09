﻿using Store.App.Infrastructure.Database.DbEntities;

namespace Store.App.Infrastructure.Database.DbRepository.Filial
{
    public class FilialRepository : GenericRepository<FilialEntity>, IFilialRepository
    {
        public FilialRepository(StoreContext db_dbContext) : base(db_dbContext)
        {
        }
    }
}
