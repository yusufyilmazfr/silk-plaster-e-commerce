using SilkPlaster.BusinessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SilkPlaster.UI.Controllers
{
    public class SliderController : Controller
    {
        SliderManager _sliderManager = new SliderManager();

        public PartialViewResult Index()
        {
            return PartialView(_sliderManager.GetAll());
        }
    }
}