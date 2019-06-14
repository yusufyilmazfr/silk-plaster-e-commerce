using SilkPlaster.Core.Abstract;
using SilkPlaster.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SilkPlaster.DataAccessLayer.EntityFramework
{
    public class Repository<T> : IRepository<T> where T : class, IEntity, new()
    {
        DatabaseContext _context = new DatabaseContext();
        public DbSet<T> _objectSet { get; set; }

        public Repository()
        {
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
