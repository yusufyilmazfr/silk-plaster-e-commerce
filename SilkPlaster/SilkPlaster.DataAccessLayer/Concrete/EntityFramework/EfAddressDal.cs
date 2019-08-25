using SilkPlaster.DataAccessLayer.Abstract;
using SilkPlaster.DataAccessLayer.Concrete.EntityFramework.Context;
using SilkPlaster.DataAccessLayer.Concrete.EntityFramework.Repository;
using SilkPlaster.Entities;
using SilkPlaster.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilkPlaster.DataAccessLayer.Concrete.EntityFramework
{
    public class EfAddressDal : EfEntityRepository<Address>, IAddressDal
    {
        public EfAddressDal(DbContext context) : base(context)
        {

        }
    }
}
