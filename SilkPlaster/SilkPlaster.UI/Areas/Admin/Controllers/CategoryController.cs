using System;
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
using SilkPlaster.UI.Models.Filters;

namespace SilkPlaster.UI.Areas.Admin.Controllers
{
    //[AdminAuthFilter]
    public class CategoryController : Controller
    {
        CategoryManager _categoryManager = new CategoryManager();

        public ActionResult Index()
        {
            return View(_categoryManager.GetAll());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Category category)
        {
            ModelState.Remove("AddedDate");

            if (ModelState.IsValid)
            {
                BusinessLayerResult<Category> layerResult = _categoryManager.Insert(category);

                if (layerResult.Errors.Count > 0)
                {
                    layerResult.Errors.ForEach(x => ModelState.AddModelError("", x.ErrorMessage));
                    return View(category);
                }

                return RedirectToAction("Index");
            }

            return View(category);
        }

        public ActionResult Edit(int? Id)
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Category category = _categoryManager.Find(i => i.Id == Id.Value);

            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        [HttpPost]
        public ActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                BusinessLayerResult<Category> layerResult = _categoryManager.Update(category);

                if (layerResult.Errors.Count > 0)
                {
                    layerResult.Errors.ForEach(x => ModelState.AddModelError("", x.ErrorMessage));
                    return View(category);
                }

                return RedirectToAction("Index");
            }
            return View(category);
        }

        public ActionResult Delete(int? Id)
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = _categoryManager.Find(i => i.Id == Id.Value);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        [HttpPost]
        public ActionResult Delete(int Id)
        {
            Category category = _categoryManager.Find(i => i.Id == Id);
            _categoryManager.Delete(category);
            return RedirectToAction("Index");
        }
    }
}
