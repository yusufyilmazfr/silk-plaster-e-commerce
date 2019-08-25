using SilkPlaster.BusinessLayer.Concrete.Result;
using SilkPlaster.Entities;
using SilkPlaster.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilkPlaster.BusinessLayer.Abstract
{
    public interface IProductManager
    {
        Product GetProductById(int Id);
        Product GetProductWithCommentAndImages(int productId);
        Product GetProductDetails(int productId);
        BusinessLayerResult<Product> Update(Product product);
        BusinessLayerResult<Product> Insert(Product obj);
        List<Product> GetProductsWithDetails();
        List<Product> GetBestSellers(int productCount);
        List<Product> GetNewProducts(int productCount);
        List<Product> GetProductsWithCategoriesDesc();
        List<Product> GetMostCommentedProducts(int productCount);
        List<Product> GetFeaturedProducts(int productCount);
        Product GetProductWithImages(int productId);
        int RemoveProduct(Product product);
        int GetAllProductCount();
        void DoSomething();
    }
}
