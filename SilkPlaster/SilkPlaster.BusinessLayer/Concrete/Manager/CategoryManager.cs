using SilkPlaster.BusinessLayer.Abstract;
using SilkPlaster.BusinessLayer.Concrete.Result;
using SilkPlaster.Common.HelperClasses;
using SilkPlaster.Common.Message;
using SilkPlaster.DataAccessLayer.Abstract;
using SilkPlaster.DataAccessLayer.Abstract.UnitOfWork;
using SilkPlaster.Entities;
using SilkPlaster.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilkPlaster.BusinessLayer.Concrete.Manager
{
    public class CategoryManager : ICategoryManager
    {
        private ICategoryDal _categoryDal { get; set; }
        private IUnitOfWork _unitOfWork { get; set; }
        private BusinessLayerResult<Category> _layerResult { get; set; }

        public CategoryManager(ICategoryDal categoryDal,IUnitOfWork unitOfWork)
        {
            _categoryDal = categoryDal;
            _unitOfWork = unitOfWork;
            _layerResult = new BusinessLayerResult<Category>();
        }

        public List<Category> GetAll()
        {
            return _categoryDal.GetAll();
        }

        public BusinessLayerResult<Category> UpdateCategory(Category obj)
        {
            Category category = GetCategoryById(obj.Id);

            if (ObjectHelper.ObjectIsNull(category))
            {
                _layerResult.AddError(ErrorMessageCode.ObjectNotFound, "Böyle bir kategori bulunmamaktadır!");
                return _layerResult;
            }

            category.Name = obj.Name;
            category.Description = obj.Description;

            _categoryDal.Update(category);

            int count = _unitOfWork.SaveChanges();

            if (count == 0)
            {
                _layerResult.AddError(ErrorMessageCode.FailedToModifiedRecord, "Kategori düzenlenemedi!");
            }

            return _layerResult;
        }

        public BusinessLayerResult<Category> AddCategory(Category category)
        {
            _categoryDal.Insert(category);

            int count = _unitOfWork.SaveChanges();

            if (count > 0)
            {
                _layerResult.Result = category;
            }
            return _layerResult;
        }

        public Category GetCategoryById(int categoryId)
        {
            return _categoryDal.Find(i => i.Id == categoryId);
        }

        public int RemoveCategory(Category category)
        {
            _categoryDal.Delete(category);
            return _unitOfWork.SaveChanges();
        }

        public List<Category> GetCategoriesWithProducts()
        {
            return _categoryDal.GetCategoriesWithProducts();
        }
    }
}
