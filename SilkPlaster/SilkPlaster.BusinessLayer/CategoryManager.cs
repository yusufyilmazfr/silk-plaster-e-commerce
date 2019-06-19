using SilkPlaster.BusinessLayer.Abstract;
using SilkPlaster.BusinessLayer.Result;
using SilkPlaster.Common.Message;
using SilkPlaster.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilkPlaster.BusinessLayer
{
    public class CategoryManager : ManagerBase<Category>
    {
        BusinessLayerResult<Category> layerResult = new BusinessLayerResult<Category>();

        public new BusinessLayerResult<Category> Update(Category obj)
        {
            Category category = base.Find(i => i.Id == obj.Id);

            if (category == null)
            {
                layerResult.AddError(ErrorMessageCode.ObjectNotFound, "Böyle bir kategori bulunmamaktadır!");
                return layerResult;
            }

            category.Name = obj.Name;
            category.Description = obj.Description;

            int count = base.Update(category);

            if (count == 0)
            {
                layerResult.AddError(ErrorMessageCode.FailedToModifiedRecord, "Kategori düzenlenemedi!");
            }
            return layerResult;
        }

        public new BusinessLayerResult<Category> Insert(Category obj)
        {
            int count = base.Insert(obj);

            if (count > 0)
            {
                layerResult.Result = obj;
            }
            return layerResult;
        }
    }
}
