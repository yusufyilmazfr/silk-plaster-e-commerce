﻿using SilkPlaster.DataAccessLayer.Abstract.Repository;
using SilkPlaster.Entities;
using SilkPlaster.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilkPlaster.DataAccessLayer.Abstract
{
    public interface IProductDal : IRepository<Product>
    {
        Product GetProductWithImages(int productId);
        Product GetProductWithCommentAndImages(int productId);
        Product GetProductDetails(int productId);
        List<Product> GetProductsWithCategoriesDesc();
        List<Product> GetProductsWithDetails();
        List<Product> GetBestSellers(int productCount);
        List<Product> GetNewProducts(int productCount);
        List<Product> GetMostCommentedProducts(int productCount);
        List<Product> GetFeaturedProducts(int productCount);
    }
}
