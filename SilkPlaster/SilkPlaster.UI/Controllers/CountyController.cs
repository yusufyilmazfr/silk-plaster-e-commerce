using SilkPlaster.BusinessLayer;
using SilkPlaster.BusinessLayer.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SilkPlaster.UI.Controllers
{
    public class CountyController : Controller
    {
        private ICountyManager _countyManager { get; set; }

        public CountyController(ICountyManager countyManager)
        {
            _countyManager = countyManager;
        }

        // GET: County
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetCountyWithCities(int cityId)
        {
            var counties = _countyManager.GetAllByCityId(cityId);

            return Json(counties, JsonRequestBehavior.AllowGet);
        }
    }
}