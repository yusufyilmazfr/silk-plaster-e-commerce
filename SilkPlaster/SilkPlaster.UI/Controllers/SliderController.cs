using SilkPlaster.BusinessLayer;
using SilkPlaster.BusinessLayer.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SilkPlaster.UI.Controllers
{
    public class SliderController : Controller
    {
        private ISliderManager _sliderManager { get; set; }

        public SliderController(ISliderManager sliderManager)
        {
            _sliderManager = sliderManager;
        }

        public PartialViewResult Index()
        {
            return PartialView(_sliderManager.GetAll());
        }
    }
}