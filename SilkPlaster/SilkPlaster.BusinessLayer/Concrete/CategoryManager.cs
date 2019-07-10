﻿using SilkPlaster.BusinessLayer.Abstract;
using SilkPlaster.BusinessLayer.Concrete.Result;
using SilkPlaster.Common.HelperClasses;
using SilkPlaster.Common.Message;
using SilkPlaster.DataAccessLayer.Abstract;
using SilkPlaster.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilkPlaster.BusinessLayer.Concrete
{
    public class CategoryManager : ICategoryManager
    {
        private ICategoryDal _categoryDal { get; set; }
        private BusinessLayerResult<Category> _layerResult { get; set; }

        public CategoryManager(ICategoryDal categoryDal)
        {
            _categoryDal = categoryDal;
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

            int count = _categoryDal.Update(category);

            if (count == 0)
            {
                _layerResult.AddError(ErrorMessageCode.FailedToModifiedRecord, "Kategori düzenlenemedi!");
            }

            return _layerResult;
        }

        public BusinessLayerResult<Category> AddCategory(Category category)
        {
            int count = _categoryDal.Insert(category);

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
            return _categoryDal.Delete(category);
        }

        public List<Category> GetCategoriesWithProducts()
        {
            return _categoryDal.GetCategoriesWithProducts();
        }
    }
}