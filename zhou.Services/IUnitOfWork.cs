using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace zhou.Services
{
    public interface IUnitOfWork : IDisposable
    {
        IDbConnection _conn { get; set; }
        IDbTransaction _trans { get; set; }
        void BeginTrans();
        void Commit();
        void RollBack();
    }
}
