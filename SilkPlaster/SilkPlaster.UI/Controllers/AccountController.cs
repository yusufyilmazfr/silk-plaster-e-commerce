using SilkPlaster.BusinessLayer;
using SilkPlaster.BusinessLayer.Result;
using SilkPlaster.Common.EntityValueObjects;
using SilkPlaster.Entities;
using SilkPlaster.UI.Models;
using SilkPlaster.UI.Models.Helpers;
using SilkPlaster.UI.Models.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SilkPlaster.UI.Controllers
{
    public class AccountController : Controller
    {
        MemberManager _memberManager = new MemberManager();

        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
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

                    return RedirectToAction("Index");
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

        public ActionResult MyAddresses()
        {

            return View();
        }


        public ActionResult Logout()
        {
            CurrentSession.Remove("Member");
            return View("Login");
        }

    }
}