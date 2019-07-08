using SilkPlaster.BusinessLayer;
using SilkPlaster.BusinessLayer.Abstract;
using SilkPlaster.UI.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SilkPlaster.UI.Controllers
{
    public class CategoryController : Controller
    {
        private ICategoryManager _categoryManager { get; set; }

        public CategoryController(ICategoryManager categoryManager)
        {
            _categoryManager = categoryManager;
        }

        // GET: Category
        public ActionResult Index()
        {
            return View();
        }

        [ChildActionOnly]
        public PartialViewResult GetCategories()
        {
            List<CategoryModel> categories = _categoryManager
                .GetCategoriesWithProducts()
                .Select(i => new CategoryModel
                {
                    Id = i.Id,
                    Name = i.Name,
                    ProductCount = i.Products.Count
                }
                ).ToList();

            return PartialView(categories);
        }
    }
}