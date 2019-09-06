using System;
using System.Collections.Generic;
using System.Text;
using zhou.Models.DBModels;
using zhou.Services.Repository;

namespace zhou.Services.RepositoryImpl
{
    public class ArticleRepository : BaseRepository<Article>, IArticleRepository
    {
        public ArticleRepository(IUnitOfWork uow) : base(uow)
        {
        }
    }
}
