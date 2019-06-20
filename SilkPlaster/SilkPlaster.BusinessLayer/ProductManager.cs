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
    public class ProductManager : ManagerBase<Product>
    {
        BusinessLayerResult<Product> layerResult = new BusinessLayerResult<Product>();

        public new BusinessLayerResult<Product> Update(Product obj)
        {
            Product product = base.Find(i => i.Id == obj.Id);

            if (product == null)
            {
                layerResult.AddError(ErrorMessageCode.ObjectNotFound, "Böyle bir kategori bulunmamaktadır!");
                return layerResult;
            }

            product.Name = obj.Name;
            product.LongDescription = obj.LongDescription;
            product.ShortDescription = obj.ShortDescription;
            product.LastPrice = obj.LastPrice;
            product.NewPrice = obj.NewPrice;
            product.FirstImage = obj.FirstImage;
            product.IsContinued = obj.IsContinued;
            product.IsFeatured = obj.IsFeatured;
            product.InStock = obj.InStock;
            product.Quantity = obj.Quantity;
            product.AddedDate = obj.AddedDate;
            product.CategoryId = obj.CategoryId;

            int count = base.Update(product);

            if (count == 0)
            {
                layerResult.AddError(ErrorMessageCode.FailedToModifiedRecord, "Ürün düzenlenemedi!");
            }
            return layerResult;
        }

        public new BusinessLayerResult<Product> Insert(Product obj)
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
