using SilkPlaster.BusinessLayer.Abstract;
using SilkPlaster.Entities;
using SilkPlaster.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SilkPlaster.UI.Controllers
{
    [ValidateInput(false)]
    public class HomeController : Controller
    {
        private IInComingMailManager _inComingMailService { get; set; }
        private IMemberManager _memberManager { get; set; }

        public HomeController(IInComingMailManager inComingMailService, IMemberManager memberManager)
        {
            _inComingMailService = inComingMailService;
            _memberManager = memberManager;
        }

        public ActionResult Index()
        {
            return View();
        }

        [Route("biz-kimiz")]
        public ActionResult AboutUs()
        {
            return View();
        }

        [Route("sikca-sorulan-sorular")]
        public ActionResult FAQ()
        {
            return View();
        }

        [Route("iletisim")]
        public ActionResult Contact()
        {
            return View();
        }

        [Route("iletisim")]
        [HttpPost]
        public ActionResult Contact(InComingMail model)
        {
            ModelState.Remove("Id");
            ModelState.Remove("AddedDate");
            ModelState.Remove("ModifiedDate");

            if (ModelState.IsValid)
            {
                int count = _inComingMailService.Insert(model);

                if (count > 0)
                    TempData["success"] = $"Sayın {model.PersonFirstName} {model.PersonLastName}, mesajınızı aldık. En yakın zamanda dönüş yapacağız. İyi günlerde kalın :)";
            }

            return View(model);
        }
    }
}