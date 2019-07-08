using SilkPlaster.DataAccessLayer.Abstract;
using SilkPlaster.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilkPlaster.DataAccessLayer.Concrete
{
    public class CategoryDal : EntityRepository<Category>, ICategoryDal
    {
        public List<Category> GetCategoriesWithProducts()
        {
            return ListQueryable().Include("Products").ToList();
        }
    }
}
