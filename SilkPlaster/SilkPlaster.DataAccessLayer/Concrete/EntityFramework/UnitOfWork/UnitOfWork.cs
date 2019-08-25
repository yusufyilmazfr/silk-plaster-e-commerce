using SilkPlaster.DataAccessLayer.Abstract.Repository;
using SilkPlaster.DataAccessLayer.Abstract.UnitOfWork;
using SilkPlaster.DataAccessLayer.Concrete.EntityFramework.Context;
using SilkPlaster.DataAccessLayer.Concrete.EntityFramework.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilkPlaster.DataAccessLayer.Concrete.EntityFramework.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private DatabaseContext DatabaseContext => (DatabaseContext)_dbContext;
        private DbContext _dbContext { get; set; }

        public UnitOfWork(DbContext dbContext)
        {
            if (dbContext == null)
                throw new Exception("DbContext can not be null!");
            _dbContext = dbContext;
        }

        public int SaveChanges()
        {
            using (var scope = _dbContext.Database.BeginTransaction())
            {
                int count = 0;
                try
                {
                    count = _dbContext.SaveChanges();
                    scope.Commit();
                    return count;
                }
                catch
                {
                    scope.Rollback();
                    return count;
                }
            }
        }

        #region Disposable Design Pattern

        private bool disposed = false;
        public void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
