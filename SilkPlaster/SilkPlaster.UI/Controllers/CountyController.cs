using SilkPlaster.BusinessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SilkPlaster.UI.Controllers
{
    public class CountyController : Controller
    {
        // GET: County
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetCountyWithCities(int cityId)
        {
            CountyManager countyManager = new CountyManager();

            var counties = countyManager
                .ListQueryable()
                .Where(i => i.CityId == cityId)
                .ToList();

            return Json(counties, JsonRequestBehavior.AllowGet);
        }
    }
}