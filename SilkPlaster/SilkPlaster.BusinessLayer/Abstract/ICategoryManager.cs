using SilkPlaster.BusinessLayer.Concrete.Result;
using SilkPlaster.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilkPlaster.BusinessLayer.Abstract
{
    public interface ICategoryManager
    {
        Category GetCategoryById(int categoryId);
        List<Category> GetAll();
        List<Category> GetCategoriesWithProducts();
        BusinessLayerResult<Category> UpdateCategory(Category category);
        BusinessLayerResult<Category> AddCategory(Category category);
        int RemoveCategory(Category category);
    }
}
