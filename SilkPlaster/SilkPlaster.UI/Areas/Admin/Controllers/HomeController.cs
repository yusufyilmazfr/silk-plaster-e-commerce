using SilkPlaster.BusinessLayer.Abstract;
using SilkPlaster.Common.EntityValueObjects;
using SilkPlaster.Common.HelperClasses;
using SilkPlaster.Entities;
using SilkPlaster.UI.Models.Filters;
using SilkPlaster.UI.Models.Helpers;
using SilkPlaster.UI.Models.Helpers.Session;
using SilkPlaster.UI.Models.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SilkPlaster.UI.Areas.Admin.Controllers
{
    //[AdminAuthFilter]
    public class HomeController : Controller
    {
        private IAdminManager _adminManager { get; set; }

        public HomeController(IAdminManager adminManager)
        {
            _adminManager = adminManager;
        }

        [AdminAuthFilter]
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Login(string returnUrl = "/Admin/Home")
        {
            TempData["returnUrl"] = returnUrl;

            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var admin = _adminManager.GetAdminWithEmailAndPassword(model.Email, model.Password);

                if (ObjectHelper.ObjectIsNull(admin))
                {
                    ModelState.AddModelError("", "Böyle bir kullanıcı bulunmamaktadır!");
                    return View(model);
                }
                CurrentSession.Set<AdminSessionModel>("Admin", new AdminSessionModel
                {
                    Id = admin.Id,
                    Email = admin.Email,
                    FirstName = admin.FirstName,
                    LastName = admin.LastName
                });


                string returnUrl = TempData["returnUrl"].ToString();
                return Redirect(returnUrl);
            }
            return View(model);
        }
    }
}