using SilkPlaster.BusinessLayer.Abstract;
using SilkPlaster.BusinessLayer.Concrete.Result;
using SilkPlaster.Common.EntityValueObjects;
using SilkPlaster.Common.HelperClasses;
using SilkPlaster.Common.SearchFiltering;
using SilkPlaster.Entities;
using SilkPlaster.UI.Models;
using SilkPlaster.UI.Models.Filters;
using SilkPlaster.UI.Models.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace SilkPlaster.UI.Controllers
{
    public class ProductController : Controller
    {
        private IProductManager _productManager { get; set; }
        private IOrderDetailManager _orderDetailManager { get; set; }
        private ICommentManager _commentManager { get; set; }

        public ProductController(IProductManager productManager, IOrderDetailManager orderDetailManager, ICommentManager commentManager)
        {
            _productManager = productManager;
            _orderDetailManager = orderDetailManager;
            _commentManager = commentManager;
        }

        // GET: Product
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Details(int? Id)
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Product product = _productManager.GetProductDetails(Id.Value);

            if (product == null)
            {
                return HttpNotFound();
            }

            ProductDetailsModel productDetails = new ProductDetailsModel
            {
                Id = product.Id,
                Name = product.Name,
                LastPrice = product.LastPrice,
                NewPrice = product.NewPrice,
                FirstImage = product.FirstImage,
                InStock = product.InStock,
                IsContinued = product.InStock,
                ShortDescription = product.ShortDescription,
                LongDescription = product.LongDescription,
                AddedDate = product.AddedDate,

                Images = product.ProductImages.Select(k => new ProductImagesModel
                {
                    Name = k.Name
                }).ToList(),

                Comments = product.Comments.Where(x => x.IsValid).Select(j => new CommentModel
                {
                    Id = j.Id,
                    Text = j.Text,
                    StarCount = j.StarCount,
                    AddedDate = j.AddedDate,
                    ParentId = j.ParentId,

                    Member = new MemberDetailsModel
                    {
                        Id = j.Member.Id,
                        FirstName = j.Member.FirstName,
                        LastName = j.Member.LastName
                    }

                }).ToList()
            };

            return View(productDetails);
        }

        public ActionResult Search(ProductFilter filter, string q)
        {
            filter.IsContinued = true;

            //will be refactoring here :)

            BinaryExpression binaryExpression = null;
            Expression defaultExpression = Expression.Constant(true);

            var personList = _productManager.GetProductsWithDetails();
            var parameter = Expression.Parameter(typeof(Product));

            var filterList = filter.GetType().GetProperties()
                                   .Select(prop =>
                                   new
                                   {
                                       name = prop.Name,
                                       value = prop.GetValue(filter, null)

                                   }).Where(x => x.value != null).ToList();

            filterList.ForEach(item =>
            {
                binaryExpression = Expression.AndAlso(binaryExpression == null ? defaultExpression : binaryExpression, Expression.Equal
                                                   (
                                                       Expression.PropertyOrField(parameter, item.name),
                                                       Expression.Constant(item.value)
                                                   ));
            });


            var filteredName = "Name";

            // contains method
            System.Reflection.MethodInfo containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });

            // reference a field
            var fieldExpression = Expression.PropertyOrField(parameter, filteredName);

            // your value
            var valueExpression = Expression.Constant(q ?? "");

            binaryExpression = Expression.AndAlso(binaryExpression == null ? defaultExpression : binaryExpression, Expression.Call
                (
                    fieldExpression, containsMethod, valueExpression
                ));


            var finalExpression = Expression.Lambda<Func<Product, bool>>(binaryExpression, parameter);


            //will be refactoring here, BAD CODE, create a extract method :(
            var result = personList.AsQueryable().Where(finalExpression).ToList() as List<Product>;
            //var result = personList.AsQueryable().Where(finalExpression).Where(i => i.Name.Contains(q ?? "")).ToList() as List<Product>;

            #region last product search model

            //page = page < 1 ? 1 : page;

            //List<ProductDetailsModel> products = _productManager
            //    .ListQueryable()
            //    .Where(i => i.InStock && i.IsContinued && (i.Name.Contains(q) || i.ShortDescription.Contains(q) || i.LongDescription.Contains(q)))
            //    .Select(i => new ProductDetailsModel
            //    {
            //        Id = i.Id,
            //        Name = i.Name,
            //        FirstImage = i.FirstImage,
            //        LastPrice = i.LastPrice,
            //        NewPrice = i.NewPrice,
            //        AddedDate = i.AddedDate,

            //        Comments = i.Comments.Where(x => x.IsValid).Select(j => new CommentModel
            //        {
            //            StarCount = j.StarCount
            //        }).ToList()
            //    })
            //    .OrderByDescending(z => z.AddedDate)
            //    .Skip((page - 1) * 12)
            //    .Take(12)
            //    .ToList();

            //ViewBag.ProductCount = _productManager.GetAllProductCount();

            #endregion


            return View(result);
        }

        public PartialViewResult QuicklyViewProduct(int Id)
        {
            Product product = _productManager.GetProductDetails(Id);

            ProductDetailsModel productDetails = new ProductDetailsModel()
            {
                Id = product.Id,
                Name = product.Name,
                NewPrice = product.NewPrice,
                LastPrice = product.LastPrice,
                FirstImage = product.FirstImage,
                ShortDescription = product.ShortDescription,

                Images = product.ProductImages.Select(j => new ProductImagesModel
                {
                    Name = j.Name
                }).ToList(),

                Comments = product.Comments.Where(x => x.IsValid).Select(j => new CommentModel
                {
                    StarCount = j.StarCount
                }).ToList()
            };

            return PartialView(productDetails);
        }

        [ChildActionOnly]
        public PartialViewResult FeaturedProducts()
        {
            var productsList = _productManager.GetFeaturedProducts(100);
            List<ProductDetailsModel> products = null;

            if (!ObjectHelper.ObjectIsNull(productsList))
            {
                products = productsList.Select(i => new ProductDetailsModel
                {
                    Id = i.Id,
                    Name = i.Name,
                    FirstImage = i.FirstImage,
                    LastPrice = i.LastPrice,
                    NewPrice = i.NewPrice,
                    AddedDate = i.AddedDate,
                }).ToList();
            }

            return PartialView(products);
        }

        [ChildActionOnly]
        public PartialViewResult BestSeller()
        {
            List<ProductDetailsModel> products = _productManager
                    .GetBestSellers(20)
                    .Select(j => new ProductDetailsModel
                    {
                        Id = j.Id,
                        Name = j.Name,
                        LastPrice = j.LastPrice,
                        NewPrice = j.NewPrice,
                        FirstImage = j.FirstImage
                    })
                .ToList();

            return PartialView(products);
        }


        [ChildActionOnly]
        public PartialViewResult MixedFeaturedProducts()
        {
            return PartialView();
        }

        [ChildActionOnly]
        public PartialViewResult GetSomeFeaturedProducts()
        {
            ViewBag.Title = "Öne Çıkarılanlar";

            List<ProductDetailsModel> products = products = _productManager
                        .GetFeaturedProducts(3)
                        .Select(i => new ProductDetailsModel()
                        {
                            Id = i.Id,
                            Name = i.Name,
                            FirstImage = i.FirstImage,
                            LastPrice = i.LastPrice,
                            NewPrice = i.NewPrice,

                            Comments = i.Comments.Where(x => x.IsValid).Select(j => new CommentModel
                            {
                                StarCount = j.StarCount
                            }).ToList()

                        }).ToList();


            return PartialView("ShowSomeProducts", products);
        }

        [ChildActionOnly]
        public PartialViewResult GetNewProducts()
        {
            ViewBag.Title = "Yeni Ürünler";

            List<ProductDetailsModel> products = _productManager.GetNewProducts(3)
                .Select(i => new ProductDetailsModel
                {
                    Id = i.Id,
                    Name = i.Name,
                    FirstImage = i.FirstImage,
                    LastPrice = i.LastPrice,
                    NewPrice = i.NewPrice,

                    Comments = i.Comments.Where(x => x.IsValid).Select(j => new CommentModel
                    {
                        StarCount = j.StarCount
                    }).ToList()

                })
                .ToList();

            return PartialView("ShowSomeProducts", products);
        }

        [ChildActionOnly]
        public PartialViewResult GetBestProducts()
        {
            ViewBag.Title = "En Çok İncelenen Ürünler";

            List<ProductDetailsModel> products =
                _productManager
                .GetMostCommentedProducts(3)
                .Select(i => new ProductDetailsModel
                {
                    Id = i.Id,
                    Name = i.Name,
                    FirstImage = i.FirstImage,
                    LastPrice = i.LastPrice,
                    NewPrice = i.NewPrice,

                    Comments = i.Comments.Where(x => x.IsValid).Select(j => new CommentModel
                    {
                        StarCount = j.StarCount
                    }).ToList()

                }).ToList();

            return PartialView("ShowSomeProducts", products);

        }

        [HttpPost]
        [MemberAuthFilter]
        public JsonResult AddComment(CommentViewModel model)
        {
            if (ModelState.IsValid)
            {
                BusinessLayerResult<Comment> layerResult = _commentManager.AddComment(new Comment
                {
                    Text = model.Text,
                    StarCount = (byte)model.StarCount,
                    ProductId = model.ProductId,
                    MemberId = CurrentSession.Member.Id,
                });

                if (layerResult.Errors.Count == 0)
                {
                    return Json(new { result = true }, JsonRequestBehavior.AllowGet);
                }

            }
            return Json(new { result = false }, JsonRequestBehavior.AllowGet);
        }
    }
}