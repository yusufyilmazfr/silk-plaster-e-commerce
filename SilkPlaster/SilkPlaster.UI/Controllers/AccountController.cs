using SilkPlaster.BusinessLayer;
using SilkPlaster.BusinessLayer.Result;
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
        MemberManager _memberManager = new MemberManager();

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
                Member member = _memberManager.Find(i => i.Email == model.Email && i.Password == model.Password);

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

            MemberDetailsModel model = _memberManager
                .ListQueryable()
                .Where(i => i.Id == loggedInMemberId)
                .Select(i => new MemberDetailsModel
                {
                    FirstName = i.FirstName,
                    LastName = i.LastName,
                    Email = i.Email
                })
                .FirstOrDefault();

            return View(model);
        }

        [HttpPost]
        [MemberAuthFilter]
        public ActionResult EditMyInformation(MemberDetailsModel model)
        {
            if (ModelState.IsValid)
            {
                int loggedInMemberId = CurrentSession.Member.Id;

                Member member = _memberManager.Find(i => i.Id == loggedInMemberId);

                member.FirstName = model.FirstName;
                member.LastName = model.LastName;
                member.Email = model.Email;


                BusinessLayerResult<Member> layerResult = _memberManager.Update(member);

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

            MemberPasswordModel model = _memberManager
                .ListQueryable()
                .Where(i => i.Id == loggedInMemberId)
                .Select(i => new MemberPasswordModel
                {
                    Password = i.Password
                })
                .FirstOrDefault();

            return View(model);
        }

        [HttpPost]
        [MemberAuthFilter]
        public ActionResult EditPassword(MemberPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                int loggedInMemberId = CurrentSession.Member.Id;

                Member member = _memberManager.Find(i => i.Id == loggedInMemberId && i.Password == model.Password);

                if (member == null)
                {
                    ModelState.AddModelError("", "Geçersiz şifre!");
                    return View(model);
                }

                member.Password = model.NewPassword;

                BusinessLayerResult<Member> layerResult = _memberManager.Update(member);

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
        public ActionResult MyAddresses()
        {
            int loggedInMemberId = CurrentSession.Member.Id;

            AddressManager addressManager = new AddressManager();

            List<AddressModel> addressModel = addressManager
                .ListQueryable()
                .Where(i => i.MemberId == loggedInMemberId)
                .Select(i => new AddressModel
                {
                    Id = i.Id,
                    Name = i.Name,
                    Description = i.Description
                })
                .ToList();

            return View(addressModel);
        }

        [MemberAuthFilter]
        public ActionResult CreateNewAddress()
        {
            CityManager cityManager = new CityManager();

            ViewBag.Cities = new SelectList(cityManager.GetAll(), "Id", "Name");

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

            CityManager cityManager = new CityManager();
            ViewBag.Cities = new SelectList(cityManager.GetAll(), "Id", "Name");

            if (ModelState.IsValid)
            {
                AddressManager addressManager = new AddressManager();

                BusinessLayerResult<Address> layerResult = addressManager.Insert(model);

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

            AddressManager addressManager = new AddressManager();
            CityManager cityManager = new CityManager();
            CountyManager countyManager = new CountyManager();

            int loggedInMemberId = CurrentSession.Member.Id;

            BusinessLayerResult<AddressViewModel> layerResult = addressManager.Find(i => i.Id == Id && i.MemberId == loggedInMemberId);

            if (layerResult.Result == null)
            {
                return HttpNotFound();
            }

            ViewBag.Cities = new SelectList(cityManager.GetAll(), "Id", "Name");
            ViewBag.Counties = new SelectList(countyManager.GetAll(i => i.CityId == layerResult.Result.CityId), "Id", "Name");

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
                AddressManager addressManager = new AddressManager();

                BusinessLayerResult<AddressViewModel> layerResult = addressManager.Update(model);

                if (layerResult.Errors.Count == 0)
                {
                    return RedirectToAction("MyAddresses");
                }

                layerResult.Errors.ForEach(x => ModelState.AddModelError("", x.ErrorMessage));
            }
            CityManager cityManager = new CityManager();
            CountyManager countyManager = new CountyManager();

            ViewBag.Cities = new SelectList(cityManager.GetAll(), "Id", "Name", model.CityId);
            ViewBag.Counties = new SelectList(countyManager.GetAll(i => i.CityId == model.CityId), "Id", "Name", model.CountyId);

            return View(model);
        }



        [HttpPost]
        public JsonResult DeleteAddress(int Id)
        {
            int loggedInMemberId = CurrentSession.Member.Id;

            AddressManager addressManager = new AddressManager();

            BusinessLayerResult<Address> layerResult = addressManager.Delete(new Address
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