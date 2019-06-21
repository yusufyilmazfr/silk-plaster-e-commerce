using SilkPlaster.BusinessLayer;
using SilkPlaster.BusinessLayer.Result;
using SilkPlaster.Common.Message;
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
    [MemberAuthFilter]
    public class WishListController : Controller
    {
        WishListManager _wishListManager = new WishListManager();

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Add(int productId)
        {
            if (ModelState.IsValid)
            {
                BusinessLayerResult<WishList> layerResult = _wishListManager.Insert(new WishList
                {
                    MemberId = CurrentSession.Member.Id,
                    ProductId = productId
                });

                int errorCount = layerResult.Errors.Count;

                if ((errorCount == 0 || errorCount == 1) || layerResult.Errors.Any(i => i.ErrorCode == ErrorMessageCode.ObjectAlreadyExists))
                {
                    return Json(new { result = true }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { result = false }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Remove(int productId)
        {

            if (ModelState.IsValid)
            {
                int loggedInMemberId = CurrentSession.Member.Id;

                BusinessLayerResult<WishList> layerResult = _wishListManager.Delete(new WishList
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

        public PartialViewResult MyWishList()
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

        public PartialViewResult GetMyWishListCount()
        {
            int loggedInMemberId = CurrentSession.Member.Id;

            int count = _wishListManager
                .ListQueryable()
                .Include("Product")
                .Include("Member")
                .Where(i => i.Member.Id == loggedInMemberId)
                .Count();

            return PartialView(count);
        }
    }
}