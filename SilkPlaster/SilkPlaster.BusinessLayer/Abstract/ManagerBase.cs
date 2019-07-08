using SilkPlaster.DataAccessLayer.Abstract;
using SilkPlaster.DataAccessLayer.Concrete;
using SilkPlaster.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SilkPlaster.BusinessLayer.Abstract
{
    public abstract class ManagerBase<T> : IRepository<T> where T : class, IEntity, new()
    {
        private EntityRepository<T> _repository { get; set; }

        public ManagerBase()
        {
            _repository = new EntityRepository<T>();
        }

        public virtual int Delete(T obj)
        {
            return _repository.Delete(obj);
        }

        public virtual T Find(Expression<Func<T, bool>> where)
        {
            return _repository.Find(where);
        }

        public virtual List<T> GetAll()
        {
            return _repository.GetAll();
        }

        public virtual List<T> GetAll(Expression<Func<T, bool>> where)
        {
            return _repository.GetAll(where);
        }

        public virtual int Insert(T obj)
        {
            return _repository.Insert(obj);
        }

        public virtual IQueryable<T> ListQueryable()
        {
            return _repository.ListQueryable();
        }

        public virtual int Save()
        {
            return _repository.Save();
        }

        public virtual int Update(T obj)
        {
            return _repository.Update(obj);
        }

    }
}
