using SilkPlaster.DataAccessLayer.Abstract.Repository;
using SilkPlaster.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilkPlaster.DataAccessLayer.Abstract.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        int SaveChanges();
    }
}
