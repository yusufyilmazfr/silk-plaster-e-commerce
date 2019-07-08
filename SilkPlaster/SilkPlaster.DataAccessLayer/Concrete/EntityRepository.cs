using SilkPlaster.DataAccessLayer.Abstract;
using SilkPlaster.DataContext.Concrete;
using SilkPlaster.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SilkPlaster.DataAccessLayer.Concrete
{
    public class EntityRepository<T> : IRepository<T> where T : class, IEntity, new()
    {
        private DatabaseContext _context { get; set; }
        private DbSet<T> _objectSet { get; set; }

        public EntityRepository()
        {
            _context = new DatabaseContext();
            _objectSet = _context.Set<T>();
        }

        public int Delete(T obj)
        {
            _objectSet.Remove(obj);
            return Save();
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

        public int Insert(T obj)
        {
            if (obj is EntityBase)
            {
                EntityBase o = obj as EntityBase;
                o.AddedDate = DateTime.Now;
            }

            _objectSet.Add(obj);
            return Save();
        }

        public IQueryable<T> ListQueryable()
        {
            return _objectSet.AsQueryable<T>();
        }

        public int Save()
        {
            return _context.SaveChanges();
        }

        public int Update(T obj)
        {
            if (obj is EntityBase)
            {
                EntityBase o = obj as EntityBase;
                o.ModifiedDate = DateTime.Now;
            }

            return Save();
        }
    }
}
