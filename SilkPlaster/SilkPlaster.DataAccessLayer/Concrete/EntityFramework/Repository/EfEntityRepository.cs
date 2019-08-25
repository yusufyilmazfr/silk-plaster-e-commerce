using SilkPlaster.DataAccessLayer.Abstract.Repository;
using SilkPlaster.DataAccessLayer.Concrete.EntityFramework.Context;
using SilkPlaster.Entities.Abstract;
using SilkPlaster.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SilkPlaster.DataAccessLayer.Concrete.EntityFramework.Repository
{
    public class EfEntityRepository<T> : IRepository<T> where T : class, IEntity, new()
    {
        private DbContext _context { get; set; }
        private DbSet<T> _objectSet { get; set; }

        public EfEntityRepository(DbContext context)
        {
            _context = context;
            _objectSet = _context.Set<T>();
        }

        #region CRUD Operations
        public void Delete(T obj)
        {
            _objectSet.Remove(obj);
        }

        public T Find(Expression<Func<T, bool>> where)
        {
            return _objectSet.FirstOrDefault(where);
        }

        public List<T> GetAll()
        {
            return _objectSet.ToList();
        }

        public List<T> GetAll(Expression<Func<T, bool>> where)
        {
            return _objectSet.Where(where).ToList();
        }

        public void Insert(T obj)
        {
            if (obj is EntityBase)
            {
                EntityBase o = obj as EntityBase;
                o.AddedDate = DateTime.Now;
            }

            _objectSet.Add(obj);

        }

        public IQueryable<T> ListQueryable()
        {
            return _objectSet.AsQueryable<T>();
        }

        public void Update(T obj)
        {
            if (obj is EntityBase)
            {
                EntityBase o = obj as EntityBase;
                o.ModifiedDate = DateTime.Now;
            }
        } 
        #endregion
    }
}
