using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SilkPlaster.Core.Abstract
{
    public interface IRepository<T>
    {
        List<T> GetAll();

        List<T> GetAll(Expression<Func<T, bool>> where);

        IQueryable<T> ListQueryable();

        T Find(Expression<Func<T, bool>> where);

        int Insert(T obj);

        int Update(T obj);

        int Delete(T obj);

        int Save();
    }
}
