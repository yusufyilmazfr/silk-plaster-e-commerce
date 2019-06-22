using SilkPlaster.BusinessLayer;
using SilkPlaster.BusinessLayer.Result;
using SilkPlaster.Entities;
using SilkPlaster.UI.Models;
using SilkPlaster.UI.Models.Filters;
using SilkPlaster.UI.Models.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SilkPlaster.UI.Controllers
{
    public class BasketController : Controller
    {
        WishListManager _wishListManager = new WishListManager();
        BasketManager _basketManager = new BasketManager();

        [MemberAuthFilter]
        public ActionResult Index()
        {
            return View();
        }

        [MemberAuthFilter]
        public PartialViewResult MyBasket()
        {
            int loggedInMemberId = CurrentSession.Member.Id;

            List<BasketModel> baskets = _basketManager
                .ListQueryable()
                .Include("Product")
                .Include("Member")
                .Where(i => i.MemberId == loggedInMemberId)
                .Select(i => new BasketModel
                {
                    Id = i.Id,
                    Quantity = i.ProductCount,
                    Product = new ProductDetailsModel
                    {
                        Id = i.Product.Id,
                        Name = i.Product.Name,
                        NewPrice = i.Product.NewPrice,
                        FirstImage = i.Product.FirstImage
                    }
                }).ToList()

                .ToList();

            return PartialView(baskets);
        }

        [MemberAuthFilter]
        public PartialViewResult GetBasketItemCount()
        {
            int loggedInMemberId = CurrentSession.Member.Id;

            int count = _basketManager
                .ListQueryable()
                .Include("Product")
                .Include("Member")
                .Where(i => i.Member.Id == loggedInMemberId)
                .Count();

            return PartialView(count);
        }

        [MemberAuthFilter]
        public PartialViewResult WiewQuicklyBasket()
        {
            int loggedInMemberId = CurrentSession.Member.Id;

            List<BasketModel> baskets = _basketManager
                .ListQueryable()
                .Include("Product")
                .Include("Member")
                .Where(i => i.Member.Id == loggedInMemberId)
                .Select(j => new BasketModel
                {
                    Id = j.Id,
                    Quantity = j.ProductCount,
                    Product = new ProductDetailsModel
                    {
                        Id = j.Product.Id,
                        Name = j.Product.Name,
                        NewPrice = j.Product.NewPrice,
                        FirstImage = j.Product.FirstImage
                    }
                })
                .ToList();


            return PartialView(baskets);
        }

        [MemberAuthFilter]
        public PartialViewResult WiewQuicklyWishList()
        {
            int loggedInMemberId = CurrentSession.Member.Id;

            List<ProductDetailsModel> products = _wishListManager
                .ListQueryable()
                .Include("Product")
                .Include("Member")
                .Where(i => i.Member.Id == loggedInMemberId)
                .Select(i => new ProductDetailsModel
                {
                    Id = i.Product.Id,
                    Name = i.Product.Name,
                    FirstImage = i.Product.FirstImage,
                    NewPrice = i.Product.NewPrice
                }).ToList();

            return PartialView(products);
        }

        [MemberAuthFilter]
        public JsonResult Remove(int productId)
        {
            if (ModelState.IsValid)
            {
                int loggedInMemberId = CurrentSession.Member.Id;

                BusinessLayerResult<Basket> layerResult = _basketManager.Delete(new Basket
                {
                    ProductId = productId,
                    MemberId = loggedInMemberId
                });

                if (layerResult.Errors.Count == 0)
                {
                    return Json(new { result = true }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { result = false, message = layerResult.Errors.FirstOrDefault() }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = false, message = "lütfen bütün alanları doldurunuz!" }, JsonRequestBehavior.AllowGet);

        }

    }
}