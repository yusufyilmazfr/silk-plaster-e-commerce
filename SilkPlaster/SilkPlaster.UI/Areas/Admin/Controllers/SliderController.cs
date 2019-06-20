﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SilkPlaster.BusinessLayer;
using SilkPlaster.BusinessLayer.Result;
using SilkPlaster.Entities;
using SilkPlaster.UI.Models;
using SilkPlaster.UI.Models.Helpers.Image;

namespace SilkPlaster.UI.Areas.Admin.Controllers
{
    public class SliderController : Controller
    {
        SliderManager _sliderManager = new SliderManager();

        public ActionResult Index()
        {
            return View(_sliderManager.GetAll());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Slider slider, HttpPostedFileBase file)
        {
            ModelState.Remove("Image");

            if (ModelState.IsValid)
            {
                string sliderAbsolutePath = Server.MapPath("~/images/sliders/");

                ImageUploadResultMessage message = ImageHelper.Save(file, sliderAbsolutePath);

                if (message.Result)
                {
                    slider.Image = message.FileName;

                    BusinessLayerResult<Slider> result = _sliderManager.Insert(slider);

                    if (result.Errors.Count > 0)
                    {
                        result.Errors.ForEach(x => ModelState.AddModelError("", x.ErrorMessage));
                        return View(slider);
                    }

                    return RedirectToAction("Index");
                }

                return View(slider);
            }

            return View(slider);
        }

        public ActionResult Edit(int? Id)
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Slider slider = _sliderManager.Find(i => i.Id == Id.Value);

            if (slider == null)
            {
                return HttpNotFound();
            }
            return View(slider);
        }

        [HttpPost]
        public ActionResult Edit(Slider slider, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                string sliderAbsolutePath = Server.MapPath("~/images/sliders/");

                ImageUploadResultMessage message = ImageHelper.Save(file, sliderAbsolutePath);

                if (message.Result)
                {
                    ImageHelper.Remove(slider.Image);
                    slider.Image = message.FileName;
                }

                BusinessLayerResult<Slider> layerResult = _sliderManager.Update(slider);

                if (layerResult.Errors.Count > 0)
                {
                    layerResult.Errors.ForEach(x => ModelState.AddModelError("", x.ErrorMessage));
                    return View(slider);
                }

                return RedirectToAction("Index");

            }
            return View(slider);
        }

        public ActionResult Delete(int? Id)
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Slider slider = _sliderManager.Find(i => i.Id == Id.Value);

            if (slider == null)
            {
                return HttpNotFound();
            }
            return View(slider);
        }

        [HttpPost]
        public ActionResult Delete(int Id)
        {

            Slider slider = _sliderManager.Find(i => i.Id == Id);

            ImageHelper.Remove(slider.Image);
            _sliderManager.Delete(slider);

            return RedirectToAction("Index");
        }
    }
}