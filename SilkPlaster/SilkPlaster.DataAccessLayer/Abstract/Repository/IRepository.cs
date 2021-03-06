﻿using SilkPlaster.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SilkPlaster.DataAccessLayer.Abstract.Repository
{
    public interface IRepository<T> where T : class, IEntity, new()
    {
        List<T> GetAll();

        List<T> GetAll(Expression<Func<T, bool>> where);

        IQueryable<T> ListQueryable();

        T Find(Expression<Func<T, bool>> where);

        void Insert(T obj);

        void Update(T obj);

        void Delete(T obj);
    }
}
