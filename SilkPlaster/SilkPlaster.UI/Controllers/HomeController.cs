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
        // GET: Home
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
    }
}