using SilkPlaster.BusinessLayer.Abstract;
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
    public class ProductManager : IProductManager
    {
        private IProductDal _productDal { get; set; }
        private BusinessLayerResult<Product> _layerResult { get; set; }

        public ProductManager(IProductDal productDal)
        {
            _productDal = productDal;
            _layerResult = new BusinessLayerResult<Product>();
        }

        public BusinessLayerResult<Product> Update(Product obj)
        {
            Product product = GetProductById(obj.Id);

            if (ObjectHelper.ObjectIsNull(product))
            {
                _layerResult.AddError(ErrorMessageCode.ObjectNotFound, "Böyle bir kategori bulunmamaktadır!");
                return _layerResult;
            }

            if (product.Quantity <= 0)
            {
                product.InStock = false;
                product.IsContinued = false;
                product.Quantity = 0;
            }
            else
            {
                product.IsContinued = obj.IsContinued;
                product.InStock = obj.InStock;
                product.Quantity = obj.Quantity;
            }

            product.Name = obj.Name;
            product.LongDescription = obj.LongDescription;
            product.ShortDescription = obj.ShortDescription;
            product.LastPrice = obj.LastPrice;
            product.NewPrice = obj.NewPrice;
            product.IsFeatured = obj.IsFeatured;
            product.FirstImage = obj.FirstImage;
            product.AddedDate = obj.AddedDate;
            product.CategoryId = obj.CategoryId;

            _productDal.Update(product);
            int count = _productDal.Save();

            if (count == 0)
            {
                _layerResult.AddError(ErrorMessageCode.FailedToModifiedRecord, "Ürün düzenlenemedi!");
            }
            return _layerResult;
        }

        public BusinessLayerResult<Product> Insert(Product obj)
        {
            _productDal.Insert(obj);
            int count = _productDal.Save();

            if (count > 0)
            {
                _layerResult.Result = obj;
            }
            return _layerResult;
        }

        public Product GetProductById(int Id)
        {
            return _productDal.Find(i => i.Id == Id);
        }

        public List<Product> GetProductsWithCategoriesDesc()
        {
            return _productDal.GetProductsWithCategoriesDesc();
        }

        public Product GetProductWithImages(int productId)
        {
            return _productDal.GetProductWithImages(productId);
        }

        public int RemoveProduct(Product product)
        {
            _productDal.Delete(product);
            return _productDal.Save();
        }

        public List<Product> GetMostCommentedProducts(int productCount)
        {
            return _productDal.GetMostCommentedProducts(productCount);
        }

        public List<Product> GetNewProducts(int productCount)
        {
            return _productDal.GetNewProducts(productCount);
        }

        public int GetAllProductCount()
        {
            return _productDal.GetAll(i => i.InStock && i.IsContinued).Count;
        }

        public List<Product> GetFeaturedProducts(int productCount)
        {
            return _productDal.GetFeaturedProducts(productCount);
        }

        public Product GetProductWithCommentAndImages(int productId)
        {
            return _productDal.GetProductWithCommentAndImages(productId);
        }

        public Product GetProductDetails(int productId)
        {
            return _productDal.GetProductDetails(productId);
        }

        public List<Product> GetBestSellers(int productCount)
        {
            return _productDal.GetBestSellers(productCount);
        }
    }
}
