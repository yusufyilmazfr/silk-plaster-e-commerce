using SilkPlaster.BusinessLayer;
using SilkPlaster.BusinessLayer.Result;
using SilkPlaster.Common.EntityValueObjects;
using SilkPlaster.Entities;
using SilkPlaster.UI.Models;
using SilkPlaster.UI.Models.Filters;
using SilkPlaster.UI.Models.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace SilkPlaster.UI.Controllers
{
    public class ProductController : Controller
    {
        ProductManager _productManager = new ProductManager();
        OrderDetailsManager _orderDetailsManager = new OrderDetailsManager();
        CommentManager _commentManager = new CommentManager();

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

            ProductDetailsModel product = _productManager
                .ListQueryable()
                .Include("Comments")
                .Include("Member")
                .Where(i => i.Id == Id.Value)
                .Select(i => new ProductDetailsModel
                {
                    Id = i.Id,
                    Name = i.Name,
                    LastPrice = i.LastPrice,
                    NewPrice = i.NewPrice,
                    FirstImage = i.FirstImage,
                    InStock = i.InStock,
                    IsContinued = i.InStock,
                    ShortDescription = i.ShortDescription,
                    LongDescription = i.LongDescription,
                    AddedDate = i.AddedDate,

                    Images = i.ProductImages.Select(k => new ProductImagesModel
                    {
                        Name = k.Name
                    }).ToList(),

                    Comments = i.Comments.Where(x => x.IsValid).Select(j => new CommentModel
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
                }).FirstOrDefault();


            if (product == null)
            {
                return HttpNotFound();
            }


            return View(product);
        }

        public ActionResult Search(string q, int page = 1)
        {
            page = page < 1 ? 1 : page;

            List<ProductDetailsModel> products = _productManager
                .ListQueryable()
                .Where(i => i.InStock && i.IsContinued && (i.Name.Contains(q) || i.ShortDescription.Contains(q) || i.LongDescription.Contains(q)))
                .Select(i => new ProductDetailsModel
                {
                    Id = i.Id,
                    Name = i.Name,
                    FirstImage = i.FirstImage,
                    LastPrice = i.LastPrice,
                    NewPrice = i.NewPrice,
                    AddedDate = i.AddedDate,

                    Comments = i.Comments.Where(x => x.IsValid).Select(j => new CommentModel
                    {
                        StarCount = j.StarCount
                    }).ToList()
                })
                .OrderByDescending(z => z.AddedDate)
                .Skip((page - 1) * 12)
                .Take(12)
                .ToList();

            ViewBag.ProductCount = _productManager.GetAll().Count;

            return View(products);
        }

        public PartialViewResult QuicklyViewProduct(int Id)
        {
            ProductDetailsModel product = _productManager
                .ListQueryable()
                .Include("Comment")
                .Include("ProductImages")
                .Where(i => i.Id == Id)
                .Select(i => new ProductDetailsModel
                {
                    Id = i.Id,
                    Name = i.Name,
                    NewPrice = i.NewPrice,
                    LastPrice = i.LastPrice,
                    FirstImage = i.FirstImage,
                    ShortDescription = i.ShortDescription,

                    Images = i.ProductImages.Select(j => new ProductImagesModel
                    {
                        Name = j.Name
                    }).ToList(),

                    Comments = i.Comments.Where(x => x.IsValid).Select(j => new CommentModel
                    {
                        StarCount = j.StarCount
                    }).ToList()

                })
                .FirstOrDefault();

            return PartialView(product);
        }

        [ChildActionOnly]
        public PartialViewResult FeaturedProducts()
        {
            List<ProductDetailsModel> products = _productManager
                    .ListQueryable()
                    .Where(i => i.IsFeatured && i.InStock && i.IsContinued)
                    .OrderByDescending(i => i.AddedDate)
                    .Select(i => new ProductDetailsModel
                    {
                        Id = i.Id,
                        Name = i.Name,
                        FirstImage = i.FirstImage,
                        LastPrice = i.LastPrice,
                        NewPrice = i.NewPrice,
                        AddedDate = i.AddedDate,
                    }).ToList();

            return PartialView(products);
        }

        [ChildActionOnly]
        public PartialViewResult BestSeller()
        {

            // This place will be corrected later

            // Get Most Saled Products Id
            var mostSaledProductsId = _orderDetailsManager
                .ListQueryable()
                .GroupBy(i => i.ProductId)
                .OrderByDescending(i => i.Count())
                .Take(100)
                .ToList();

            List<ProductDetailsModel> products = new List<ProductDetailsModel>();


            // I Added most saled product in products

            for (int i = 0; i < mostSaledProductsId.Count; i++)
            {
                int id = mostSaledProductsId[i].Key;

                ProductDetailsModel product = _productManager
                 .ListQueryable()
                 .Include("Comments")
                 .Where(k => k.Id == id && k.InStock && k.IsContinued)
                 .Select(j => new ProductDetailsModel
                 {
                     Id = j.Id,
                     Name = j.Name,
                     LastPrice = j.LastPrice,
                     NewPrice = j.NewPrice,
                     FirstImage = j.FirstImage
                 })
                 .FirstOrDefault();

                products.Add(product);
            }

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

            List<ProductDetailsModel> products = _productManager
                .ListQueryable()
                .Where(i => i.IsFeatured && i.IsContinued && i.IsContinued)
                .OrderByDescending(i => i.AddedDate)
                .Take(3)
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
        public PartialViewResult GetNewProducts()
        {
            ViewBag.Title = "Yeni Ürünler";

            List<ProductDetailsModel> products = _productManager
                .ListQueryable()
                .Where(i => i.InStock && i.IsFeatured && i.IsContinued)
                .OrderByDescending(i => i.AddedDate)
                .Take(3)
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

            List<ProductDetailsModel> products = _productManager.ListQueryable().OrderByDescending(i => i.Comments.Count).Take(3).Select(i => new ProductDetailsModel
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
                BusinessLayerResult<Comment> layerResult = _commentManager.Insert(new Comment
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