using System;
using System.Collections.Generic;
using System.Text;
using zhou.Models.DBModels;

namespace zhou.Services.Repository
{
    public interface IArticleRepository : IDependency, IBaseRepository<Article>
    {
    }
}
