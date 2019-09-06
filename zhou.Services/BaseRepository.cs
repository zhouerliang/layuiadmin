using DapperExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using zhou.Models;

namespace zhou.Services
{
    public abstract class BaseRepository<T> : IDisposable, IBaseRepository<T> where T : BaseEntity
    {
        //这里不在自己的实例内部创建独享的conn连接了。
        //暴露出去，在外边传进来，这样才可以在业务层共用同一个conn连接，
        //同时，跨repository的事务也得以实现
        public IUnitOfWork _uow { get; set; }
        public BaseRepository(IUnitOfWork uow)
        {
            _uow = uow;
        }
        public int Add(T t)
        {
            return _uow._conn.Insert<T>(t, _uow._trans);
        }

        public bool Del(T t)
        {
            return _uow._conn.Delete<T>(t, _uow._trans);
        }

        public T GetByID(int id)
        {
            return _uow._conn.Get<T>(id, _uow._trans);
        }

        public IEnumerable<T> GetAll()
        {
            return _uow._conn.GetList<T>(transaction: _uow._trans).ToList();
        }

        public bool Update(T t)
        {
            return _uow._conn.Update<T>(t, _uow._trans);
        }

        public IEnumerable<T> GetByCondition(object predicate)
        {
            return _uow._conn.GetList<T>(predicate, transaction: _uow._trans).ToList();
        }

        /// <summary>
        /// 条件分页查询
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="sort"></param>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        public Tuple<IEnumerable<T>, int> GetPage(object predicate, IList<ISort> sort, int pageindex, int pagesize)
        {
            return new Tuple<IEnumerable<T>, int>(_uow._conn.GetPage<T>(predicate, sort, pageindex, pagesize, _uow._trans).ToList(), _uow._conn.Count<T>(predicate, _uow._trans));
        }

        public void Dispose()
        {
            _uow.Dispose();
        }
    }
}
