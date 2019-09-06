using DapperExtensions;
using System;
using System.Collections.Generic;
using System.Text;
using zhou.Models;

namespace zhou.Services
{
    public interface IBaseRepository<T> where T : BaseEntity
    {
        IUnitOfWork _uow { get; set; }

        int Add(T t);
        bool Del(T t);
        bool Update(T t);
        T GetByID(int id);

        /// <summary>
        /// 获取所有
        /// </summary>
        /// <returns></returns>
        IEnumerable<T> GetAll();

        /// <summary>
        /// 条件查询所有
        /// </summary>
        /// <returns></returns>
        IEnumerable<T> GetByCondition(object predicate);

        /// <summary>
        /// 分页条件查询
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="sort"></param>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        Tuple<IEnumerable<T>, int> GetPage(object predicate, IList<ISort> sort, int pageindex, int pagesize);
    }
}
