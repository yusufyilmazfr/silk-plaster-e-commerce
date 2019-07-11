﻿using SilkPlaster.BusinessLayer;
using SilkPlaster.BusinessLayer.Abstract;
using SilkPlaster.BusinessLayer.Concrete.Result;
using SilkPlaster.Common.EntityValueObjects;
using SilkPlaster.Entities;
using SilkPlaster.UI.Models;
using SilkPlaster.UI.Models.Filters;
using SilkPlaster.UI.Models.Helpers;
using SilkPlaster.UI.Models.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace SilkPlaster.UI.Controllers
{
    public class AccountController : Controller
    {

        private IMemberManager _memberManager { get; set; }
        private IAddressManager _addressManager { get; set; }
        private ICityManager _cityManager { get; set; }
        private ICountyManager _countyManager { get; set; }
        private IOrderManager _orderManager { get; set; }

        public AccountController(IMemberManager memberManager, IAddressManager addressManager, ICityManager cityManager, ICountyManager countyManager, IOrderManager orderManager)
        {
            _memberManager = memberManager;
            _addressManager = addressManager;
            _cityManager = cityManager;
            _countyManager = countyManager;
            _orderManager = orderManager;
        }

        [MemberAuthFilter]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login(string returnUrl = "/")
        {
            TempData["returnUrl"] = returnUrl;

            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if (CurrentSession.Member != null)
            {
                return RedirectToAction("Login");
            }

            if (ModelState.IsValid)
            {
                Member member = _memberManager.GetMemberWithEmailAndPassword(model.Email, model.Password);

                if (member == null)
                {
                    ModelState.AddModelError("", "Hatalı E-posta veya parola");
                }
                else
                {
                    CurrentSession.Set<MemberSessionModel>("Member", new MemberSessionModel
                    {
                        Id = member.Id,
                        Email = member.Email,
                        FirstName = member.FirstName,
                        LastName = member.LastName
                    });

                    string returnUrl = TempData["returnUrl"].ToString();
                    return Redirect(returnUrl);
                }
            }

            return View(model);
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                BusinessLayerResult<Member> layerResult = _memberManager.Register(model);

                if (layerResult.Errors.Count > 0)
                {
                    layerResult.Errors.ForEach(x => ModelState.AddModelError("", x.ErrorMessage));
                    return View(model);
                }

                return RedirectToAction("Login");

            }

            return View(model);
        }

        [MemberAuthFilter]
        public ActionResult EditMyInformation()
        {
            int loggedInMemberId = CurrentSession.Member.Id;

            Member member = _memberManager.GetMemberById(loggedInMemberId);

            MemberDetailsModel model = new MemberDetailsModel()
            {
                FirstName = member.FirstName,
                LastName = member.LastName,
                Email = member.Email
            };

            return View(model);
        }

        [HttpPost]
        [MemberAuthFilter]
        public ActionResult EditMyInformation(MemberDetailsModel model)
        {
            if (ModelState.IsValid)
            {
                int loggedInMemberId = CurrentSession.Member.Id;

                Member member = _memberManager.GetMemberById(loggedInMemberId);

                member.FirstName = model.FirstName;
                member.LastName = model.LastName;
                member.Email = model.Email;


                BusinessLayerResult<Member> layerResult = _memberManager.UpdateMember(member);

                if (layerResult.Errors.Count > 0)
                {
                    layerResult.Errors.ForEach(x => ModelState.AddModelError("", x.ErrorMessage));
                    return View(model);
                }

                return View("Index");
            }

            return View(model);
        }

        [MemberAuthFilter]
        public ActionResult EditPassword()
        {
            int loggedInMemberId = CurrentSession.Member.Id;

            MemberPasswordModel model = new MemberPasswordModel();

            model.Password = _memberManager.GetMemberById(loggedInMemberId).Password;

            return View(model);
        }

        [HttpPost]
        [MemberAuthFilter]
        public ActionResult EditPassword(MemberPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                int loggedInMemberId = CurrentSession.Member.Id;

                Member member = _memberManager.GetMemberByPassword(loggedInMemberId, model.Password);

                if (member == null)
                {
                    ModelState.AddModelError("", "Geçersiz şifre!");
                    return View(model);
                }

                member.Password = model.NewPassword;

                BusinessLayerResult<Member> layerResult = _memberManager.UpdateMember(member);

                if (layerResult.Errors.Count > 0)
                {
                    layerResult.Errors.ForEach(x => ModelState.AddModelError("", x.ErrorMessage));

                    return View(model);
                }

                return View("Index");
            }

            return View(model);
        }

        [MemberAuthFilter]
        public ActionResult MyOrders()
        {
            int loggedInMemberId = CurrentSession.Member.Id;

            List<Order> orders = _orderManager.GetOrdersByMemberId(loggedInMemberId);

            return View(orders);
        }

        [MemberAuthFilter]
        public ActionResult OrderDetail(int? Id)
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            int loggedInMemberId = CurrentSession.Member.Id;

            Order order = _orderManager.GetOrderDetailByMemberId(Id.Value, loggedInMemberId);

            if (order == null)
            {
                return HttpNotFound();
            }


            return View(order);
        }

        [MemberAuthFilter]
        public ActionResult MyAddresses()
        {
            int loggedInMemberId = CurrentSession.Member.Id;

            List<AddressModel> addressModel = _addressManager
                .GetAddressesWithMemberId(loggedInMemberId)
                .Select(i => new AddressModel
                {
                    Id = i.Id,
                    Name = i.Name,
                    Description = i.Description
                }).ToList();

            return View(addressModel);
        }

        [MemberAuthFilter]
        public ActionResult CreateNewAddress()
        {
            ViewBag.Cities = new SelectList(_cityManager.GetAll(), "Id", "Name");

            return View();
        }

        [HttpPost]
        [MemberAuthFilter]
        public ActionResult CreateNewAddress(AddressViewModel model, bool billType = true)
        {
            ModelState.Remove("CountyId");

            if (billType)
            {
                ModelState.Remove("CompanyName");
                ModelState.Remove("TaxNumber");
                ModelState.Remove("TaxAdministration");
            }
            else
            {
                ModelState.Remove("CitizenshipNumber");
            }

            model.MemberId = CurrentSession.Member.Id;

            ViewBag.Cities = new SelectList(_cityManager.GetAll(), "Id", "Name");

            if (ModelState.IsValid)
            {
                BusinessLayerResult<Address> layerResult = _addressManager.Insert(model);

                if (layerResult.Errors.Count > 0)
                {
                    layerResult.Errors.ForEach(x => ModelState.AddModelError("", x.ErrorMessage));
                    return View(model);
                }

                return RedirectToAction("MyAddresses");
            }

            return View(model);
        }

        [MemberAuthFilter]
        public ActionResult EditAddress(int? Id)
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            int loggedInMemberId = CurrentSession.Member.Id;

            BusinessLayerResult<AddressViewModel> layerResult = _addressManager.Find(i => i.Id == Id && i.MemberId == loggedInMemberId);

            if (layerResult.Result == null)
            {
                return HttpNotFound();
            }

            ViewBag.Cities = new SelectList(_cityManager.GetAll(), "Id", "Name");
            ViewBag.Counties = new SelectList(_countyManager.GetAllByCityId(layerResult.Result.CityId), "Id", "Name");

            return View(layerResult.Result);
        }

        [MemberAuthFilter]
        [HttpPost]
        public ActionResult EditAddress(AddressViewModel model, bool billType = true)
        {
            if (billType)
            {
                ModelState.Remove("CompanyName");
                ModelState.Remove("TaxNumber");
                ModelState.Remove("TaxAdministration");

                model.CompanyName = null;
                model.TaxNumber = null;
                model.TaxAdministration = null;
            }
            else
            {
                ModelState.Remove("CitizenshipNumber");
                model.CitizenshipNumber = null;
            }

            if (ModelState.IsValid)
            {
                BusinessLayerResult<AddressViewModel> layerResult = _addressManager.Update(model);

                if (layerResult.Errors.Count == 0)
                {
                    return RedirectToAction("MyAddresses");
                }

                layerResult.Errors.ForEach(x => ModelState.AddModelError("", x.ErrorMessage));
            }

            ViewBag.Cities = new SelectList(_cityManager.GetAll(), "Id", "Name", model.CityId);
            ViewBag.Counties = new SelectList(_countyManager.GetAllByCityId(model.CityId), "Id", "Name", model.CountyId);

            return View(model);
        }

        [HttpPost]
        public JsonResult DeleteAddress(int Id)
        {
            int loggedInMemberId = CurrentSession.Member.Id;

            BusinessLayerResult<Address> layerResult = _addressManager.Delete(new Address
            {
                Id = Id,
                MemberId = loggedInMemberId
            });

            if (layerResult.Errors.Count == 0)
            {
                return Json(new { result = true, message = "Adres başarıyla silindi!" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { result = false, message = layerResult.Errors.FirstOrDefault().ErrorMessage }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Logout()
        {
            CurrentSession.Remove("Member");
            return View("Login");
        }
    }
}