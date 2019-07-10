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

        public int Save()
        {
            try
            {
                DbContextTransaction transaction = _context.Database.BeginTransaction();

                try
                {
                    _context.SaveChanges();
                    transaction.Commit();
                    return 1;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return 0;
                }
            }
            catch (Exception ex)
            {
                //TO DO
            }
            return 0;
        }

        public void Update(T obj)
        {
            if (obj is EntityBase)
            {
                EntityBase o = obj as EntityBase;
                o.ModifiedDate = DateTime.Now;
            }
        }
    }
}
