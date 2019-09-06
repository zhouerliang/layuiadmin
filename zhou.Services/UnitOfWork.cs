using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace zhou.Services
{
    public class UnitOfWork : IUnitOfWork
    {
        public IDbConnection _conn { get; set; }
        public IDbTransaction _trans { get; set; }

        private bool disposed;

        //实例化时，创建数据库连接
        public UnitOfWork(IConfiguration configuration)
        {
            var connStr = configuration.GetConnectionString("Default");
            _conn = new SqlConnection(connStr);
            if (_conn.State == ConnectionState.Closed)
            {
                _conn.Open();
            }
            _trans = null;
        }

        public void BeginTrans()
        {
            _trans = _conn.BeginTransaction();
        }

        public void Commit()
        {
            _trans.Commit();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void RollBack()
        {
            _trans.Rollback();
        }
        public virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    if (_conn.State == ConnectionState.Open)
                    {
                        _conn.Close();
                        _conn.Dispose();
                    }
                }
            }
            disposed = true;
        }
    }
}
