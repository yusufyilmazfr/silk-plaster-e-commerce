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
    public class EfCategoryDal : EfEntityRepository<Category>, ICategoryDal
    {
        public EfCategoryDal(DbContext context) : base(context)
        {
        }

        public List<Category> GetCategoriesWithProducts()
        {
            return ListQueryable().Include("Products").ToList();
        }
    }
}
