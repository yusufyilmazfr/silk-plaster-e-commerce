using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SilkPlaster.BusinessLayer;
using SilkPlaster.BusinessLayer.Abstract;
using SilkPlaster.BusinessLayer.Concrete.Result;
using SilkPlaster.Entities;
using SilkPlaster.UI.Models;
using SilkPlaster.UI.Models.Filters;

namespace SilkPlaster.UI.Areas.Admin.Controllers
{
    [AdminAuthFilter]
    public class CategoryController : Controller
    {
        private ICategoryManager _categoryManager { get; set; }

        public CategoryController(ICategoryManager categoryManager)
        {
            _categoryManager = categoryManager;
        }

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
                BusinessLayerResult<Category> layerResult = _categoryManager.AddCategory(category);

                if (layerResult.HasError())
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

            Category category = _categoryManager.GetCategoryById(Id.Value);

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
                BusinessLayerResult<Category> layerResult = _categoryManager.UpdateCategory(category);

                if (layerResult.HasError())
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
            Category category = _categoryManager.GetCategoryById(Id.Value);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        [HttpPost]
        public ActionResult Delete(int Id)
        {
            Category category = _categoryManager.GetCategoryById(Id);
            _categoryManager.RemoveCategory(category);
            return RedirectToAction("Index");
        }
    }
}
