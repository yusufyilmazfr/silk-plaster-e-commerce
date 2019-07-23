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
    public class ProductDal : EntityRepository<Product>, IProductDal
    {
        private IOrderDetailDal _orderDetailDal { get; set; }

        public ProductDal(IOrderDetailDal orderDetailDal)
        {
            _orderDetailDal = orderDetailDal;
        }

        public List<Product> GetFeaturedProducts(int productCount)
        {
            return ListQueryable()
                    .Include(i => i.Comments)
                    .Where(i => i.IsFeatured && i.IsContinued && i.InStock)
                    .OrderByDescending(i => i.AddedDate)
                    .Take(productCount)
                    .ToList();
        }

        public List<Product> GetMostCommentedProducts(int productCount)
        {
            return ListQueryable()
                    .Include(i => i.Comments)
                    .OrderByDescending(i => i.Comments.Count)
                    .Take(productCount)
                    .ToList();
        }

        public List<Product> GetNewProducts(int productCount)
        {
            return ListQueryable()
                    .Include(i => i.Comments)
                    .Where(i => i.InStock && i.IsContinued)
                    .OrderByDescending(i => i.AddedDate)
                    .Take(productCount)
                    .ToList();
        }

        public List<Product> GetProductsWithCategoriesDesc()
        {
            return ListQueryable()
                    .Include(i => i.Category)
                    .OrderByDescending(i => i.AddedDate)
                    .ToList();
        }

        public Product GetProductWithImages(int productId)
        {
            return ListQueryable().Include("ProductImages").FirstOrDefault(i => i.Id == productId);
        }

        public Product GetProductWithCommentAndImages(int productId)
        {
            return ListQueryable()
                    .Include("Comment")
                    .Include("ProductImages")
                    .FirstOrDefault(i => i.Id == productId);
        }

        public Product GetProductDetails(int productId)
        {
            return ListQueryable()
                    .Include(i => i.Comments)
                    .Include(i => i.Comments.Select(k => k.Member))
                     .Include(i => i.ProductImages)
                    .FirstOrDefault(i => i.Id == productId);
        }

        public List<Product> GetBestSellers(int productCount)
        {
            // This place will be corrected later

            // Get Most Saled Products Id
            var mostSaledProductsId = _orderDetailDal
                                .ListQueryable()
                                .GroupBy(i => i.ProductId)
                                .OrderByDescending(i => i.Count())
                                .Take(productCount)
                                .ToList();


            // I Added most saled product in products
            List<Product> products = new List<Product>();

            for (int i = 0; i < mostSaledProductsId.Count; i++)
            {
                int id = mostSaledProductsId[i].Key;

                products.Add(base.Find(k => k.Id == id));
                //products.Add(base.Find(k => k.Id == id && k.InStock && k.IsContinued));
            }

            return products;
        }

        public List<Product> GetProductsWithDetails()
        {
            return ListQueryable()
                .Include(i => i.ProductImages)
                .Include(i => i.Comments)
                .ToList();
        }
    }
}
