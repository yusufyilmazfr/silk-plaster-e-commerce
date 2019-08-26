using SilkPlaster.BusinessLayer.Abstract;
using SilkPlaster.BusinessLayer.Concrete.Result;
using SilkPlaster.Common.EntityValueObjects;
using SilkPlaster.Common.HelperClasses;
using SilkPlaster.Entities;
using SilkPlaster.Entities.Concrete;
using SilkPlaster.UI.Models;
using SilkPlaster.UI.Models.Filters;
using SilkPlaster.UI.Models.Helpers;
using SilkPlaster.UI.Models.Helpers.Session;
using SilkPlaster.UI.Models.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace SilkPlaster.UI.Controllers
{
    [RoutePrefix("hesabim")]
    [ValidateInput(false)]
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
        [Route("~/hesabim")]
        public ActionResult Index()
        {
            return View();
        }

        [Route("giris-yap")]
        public ActionResult Login(string returnUrl = "/")
        {
            if (CurrentSession.Member != null)
                return RedirectToAction("Index");

            TempData["returnUrl"] = returnUrl;

            return View();
        }

        [Route("giris-yap")]
        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
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

        [Route("kayit-ol")]
        public ActionResult Register()
        {
            if (CurrentSession.Member != null)
                return RedirectToAction("Index");

            return View();
        }

        [Route("~/sifremi-unuttum")]
        public ActionResult ForgetPassword()
        {
            if (CurrentSession.MemberIsLogged)
                return RedirectToAction("Index");

            return View();
        }

        [Route("~/sifremi-unuttum")]
        [HttpPost]
        public ActionResult ForgetPassword(string email)
        {
            if (String.IsNullOrEmpty(email))
            {
                ModelState.AddModelError("", "Email alanı boş geçilemez!");
                return View();
            }

            BusinessLayerResult<Member> layerResult = _memberManager.SendNewPasswordByEmail(email);

            if (layerResult.HasError())
            {
                layerResult.Errors.ForEach(x => ModelState.AddModelError("", x.ErrorMessage));
                return View();
            }

            TempData["success"] = $"Yeni parolanız {email} adresine başarıyla gönderilmiştir! Lütfen e postanızı kontrol ediniz. Mail gelmemiş ise 1 dakika sonra tekrardan deneyiniz, hiç gelmemesi durumunda ise lütfen iletişim formundan bize ulaşınız.";

            return View();
        }

        [HttpPost]
        [Route("kayit-ol")]
        public ActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                BusinessLayerResult<Member> layerResult = _memberManager.Register(model);

                if (layerResult.HasError())
                {
                    layerResult.Errors.ForEach(x => ModelState.AddModelError("", x.ErrorMessage));
                    return View(model);
                }

                Member member = _memberManager.GetMemberWithEmailAndPassword(model.Email, model.Password);

                CurrentSession.Set<MemberSessionModel>("Member", new MemberSessionModel
                {
                    Id = member.Id,
                    Email = member.Email,
                    FirstName = member.FirstName,
                    LastName = member.LastName
                });

                return RedirectToAction("Login");

            }

            return View(model);
        }

        [MemberAuthFilter]
        [Route("bilgilerimi-duzenle")]
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
        [Route("bilgilerimi-duzenle")]
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

                if (layerResult.HasError())
                {
                    layerResult.Errors.ForEach(x => ModelState.AddModelError("", x.ErrorMessage));
                    return View(model);
                }

                return View("Index");
            }

            return View(model);
        }

        [MemberAuthFilter]
        [Route("sifremi-degistir")]
        public ActionResult EditPassword()
        {
            int loggedInMemberId = CurrentSession.Member.Id;

            MemberPasswordModel model = new MemberPasswordModel();

            model.Password = _memberManager.GetMemberById(loggedInMemberId).Password;

            return View(model);
        }

        [HttpPost]
        [MemberAuthFilter]
        [Route("sifremi-degistir")]
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

                if (layerResult.HasError())
                {
                    layerResult.Errors.ForEach(x => ModelState.AddModelError("", x.ErrorMessage));

                    return View(model);
                }

                return View("Index");
            }

            return View(model);
        }

        [MemberAuthFilter]
        [Route("siparislerim")]
        public ActionResult MyOrders()
        {
            int loggedInMemberId = CurrentSession.Member.Id;

            List<Order> orders = _orderManager.GetOrdersByMemberId(loggedInMemberId);

            return View(orders);
        }

        [MemberAuthFilter]
        [Route("siparis-detaylarim-{Id}")]
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
        [Route("adreslerim")]
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
        [Route("yeni-adres-olustur")]
        public ActionResult CreateNewAddress()
        {
            ViewBag.Cities = new SelectList(_cityManager.GetAll(), "Id", "Name");

            return View();
        }

        [HttpPost]
        [MemberAuthFilter]
        [Route("yeni-adres-olustur")]
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

                if (layerResult.HasError())
                {
                    layerResult.Errors.ForEach(x => ModelState.AddModelError("", x.ErrorMessage));
                    return View(model);
                }

                return RedirectToAction("MyAddresses");
            }

            return View(model);
        }

        [MemberAuthFilter]
        [Route("adres-guncelle/{Id:int?}")]
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
        [Route("adres-guncelle")]
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

                if (!layerResult.HasError())
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

            if (!layerResult.HasError())
            {
                return Json(new { result = true, message = "Adres başarıyla silindi!" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { result = false, message = layerResult.Errors.FirstOrDefault().ErrorMessage }, JsonRequestBehavior.AllowGet);
        }

        [Route("cikis-yap")]
        public ActionResult Logout()
        {
            CurrentSession.Remove("Member");
            return View("Login");
        }
    }
}