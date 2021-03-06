﻿using SilkPlaster.BusinessLayer;
using SilkPlaster.BusinessLayer.Abstract;
using SilkPlaster.BusinessLayer.Concrete.Result;
using SilkPlaster.Common.EntityValueObjects;
using SilkPlaster.Common.HelperClasses;
using SilkPlaster.Common.OrderMessageObj;
using SilkPlaster.Entities;
using SilkPlaster.Entities.Concrete;
using SilkPlaster.UI.Models;
using SilkPlaster.UI.Models.Filters;
using SilkPlaster.UI.Models.Helpers;
using SilkPlaster.UI.Models.Helpers.Image;
using SilkPlaster.UI.Models.Helpers.Session;
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
        private IWishListManager _wishListManager { get; set; }
        private IBasketManager _basketManager { get; set; }
        private IAddressManager _addressManager { get; set; }
        private IOrderManager _orderManager { get; set; }

        public BasketController(IWishListManager wishListManager, IBasketManager basketManager, IAddressManager addressManager, IOrderManager orderManager)
        {
            _wishListManager = wishListManager;
            _basketManager = basketManager;
            _addressManager = addressManager;
            _orderManager = orderManager;
        }

        [MemberAuthFilter]
        [Route("sepetim")]
        public ActionResult Index()
        {
            return View();
        }

        [MemberAuthFilter]
        public PartialViewResult MyBasket()
        {
            int loggedInMemberId = CurrentSession.Member.Id;

            List<BasketModel> baskets = _basketManager
                .GetBasketItemsByMemberId(loggedInMemberId)
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
                }).ToList();

            ViewBag.Addresses = new SelectList(_addressManager.GetAddressesWithMemberId(loggedInMemberId), "Id", "Name");

            return PartialView(baskets);
        }

        [HttpPost]
        [MemberAuthFilter]
        public ActionResult Confirm(int? paymentType, int? addressId)
        {
            if (ObjectHelper.ObjectIsNull(paymentType) || ObjectHelper.ObjectIsNull(addressId))
            {
                ModelState.AddModelError("", "Lütfen geçerli değerler giriniz!");
                return View("Index");
            }

            int loggedInMemberId = CurrentSession.Member.Id;
            bool paymentTypeIsExists = Enum.IsDefined(typeof(EnumPaymentTypes), paymentType.Value);

            Address address = _addressManager.GetAddressWithMemberId(addressId.Value, loggedInMemberId);

            if (ObjectHelper.ObjectIsNull(address) || !paymentTypeIsExists)
            {
                ModelState.AddModelError("", "Lütfen geçerli değerler giriniz!");
                return View("Index");
            }

            EnumPaymentTypes currentPaymentType = EnumHelper.ConvertValueToEnumObject<EnumPaymentTypes, int>(paymentType.Value);

            OrderViewModel order = new OrderViewModel()
            {
                OrderState = (int)EnumOrderState.Waiting,
                AddressId = addressId.Value,
                Description = "xd",
                MemberId = loggedInMemberId,
                OrderNumber = ImageHelper.CreateUniqueString()
            };

            order.OrderDetails = new List<OrderDetailsViewModel>();

            foreach (var item in _basketManager.GetBasketItemsByMemberId(loggedInMemberId))
            {
                order.OrderDetails.Add(new OrderDetailsViewModel()
                {
                    Id = item.Id,
                    ProductId = item.ProductId,
                    Price = item.Product.NewPrice,
                    Quantity = item.ProductCount,
                });
            }

            order.PaymentType = (int)currentPaymentType;
            BusinessLayerResult<Order> layerResult = _orderManager.CheckOrder(order);

            if (!layerResult.HasError())
            {
                TempData["OrderNumber"] = "test";
                return RedirectToAction("OrderApproved", "Basket");
            }

            layerResult.Errors.ForEach(x => ModelState.AddModelError("", x.ErrorMessage));


            return RedirectToAction("Index");
        }

        [Route("sepetim/siparis-onaylandi")]
        [MemberAuthFilter]
        public ActionResult OrderApproved()
        {
            if (ObjectHelper.ObjectIsNull(TempData["OrderNumber"]))
                return RedirectToAction("Index");


            return View();
        }

        [MemberAuthFilter]
        [HttpPost]
        public JsonResult AddProductInBasket(int productId, int productCount)
        {
            int loggedInMemberId = CurrentSession.Member.Id;

            BusinessLayerResult<Basket> layerResult = _basketManager.AddProductInBasket(loggedInMemberId, productId, productCount);

            if (layerResult.HasError())
            {
                return Json(new { result = false, message = layerResult.Errors.FirstOrDefault() }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = true, message = "" }, JsonRequestBehavior.AllowGet);

        }

        [MemberAuthFilter]
        public PartialViewResult GetBasketItemCount()
        {
            int loggedInMemberId = CurrentSession.Member.Id;

            int count = _basketManager.GetBasketItemsCountByMemberId(loggedInMemberId);
            return PartialView(count);
        }

        [MemberAuthFilter]
        public PartialViewResult WiewQuicklyBasket()
        {
            int loggedInMemberId = CurrentSession.Member.Id;

            List<BasketModel> baskets = _basketManager.GetBasketItemsByMemberId(loggedInMemberId)
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
                .GetMyWishListItemsByMemberId(loggedInMemberId)
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
        public JsonResult IncreaseProductCount(int productId)
        {
            int loggedInMemberId = CurrentSession.Member.Id;

            BusinessLayerResult<Basket> layerResult = _basketManager.AddProductInBasket(loggedInMemberId, productId, 1);

            if (layerResult.HasError())
            {
                return Json(new { message = layerResult.Errors.Select(i => i.ErrorMessage).FirstOrDefault(), result = false }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { result = true });
        }

        [MemberAuthFilter]
        public JsonResult DecreaseProductCount(int productId)
        {
            int loggedInMemberId = CurrentSession.Member.Id;

            BusinessLayerResult<Basket> layerResult = _basketManager.DecreaseProductCount(loggedInMemberId, productId, 1);

            if (layerResult.HasError())
            {
                return Json(new { message = layerResult.Errors.Select(i => i.ErrorMessage).FirstOrDefault(), result = false }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { result = true });
        }

        [MemberAuthFilter]
        public JsonResult Remove(int productId)
        {
            if (ModelState.IsValid)
            {
                int loggedInMemberId = CurrentSession.Member.Id;

                BusinessLayerResult<Basket> layerResult = _basketManager.DeleteBasketItem(new Basket
                {
                    ProductId = productId,
                    MemberId = loggedInMemberId
                });

                if (!layerResult.HasError())
                {
                    return Json(new { result = true }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { result = false, message = layerResult.Errors.FirstOrDefault() }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = false, message = "lütfen bütün alanları doldurunuz!" }, JsonRequestBehavior.AllowGet);

        }
    }
}