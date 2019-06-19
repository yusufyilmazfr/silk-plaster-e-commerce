using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SilkPlaster.BusinessLayer;
using SilkPlaster.Entities;
using SilkPlaster.UI.Models;
using SilkPlaster.UI.Models.Helpers;

namespace SilkPlaster.UI.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        ProductManager _productManager = new ProductManager();
        CategoryManager _categoryManager = new CategoryManager();

        public ActionResult Index()
        {
            var products = _productManager.ListQueryable().Include(p => p.Category);
            return View(products.ToList());
        }

        public ActionResult Create()
        {
            ViewBag.Categories = new SelectList(_categoryManager.GetAll(), "Id", "Name");
            return View();
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Create(Product product, HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                string mimeType = file.ContentType;

                if (ImageHelper.IsTypeImage(mimeType))
                {
                    string extension = Path.GetExtension(file.FileName);
                    string fileName = Path.GetFileNameWithoutExtension(file.FileName);
                    string newFileNameWithExtension = fileName + ImageHelper.CreateUniqueString() + extension;

                    string path = Server.MapPath("~/images/products/") + newFileNameWithExtension;
                    file.SaveAs(path); 

                }
            }

            if (ModelState.IsValid)
            {
                //db.Products.Add(product);
                //db.SaveChanges();
                return RedirectToAction("Index");
            }


            ViewBag.Categories = new SelectList(_categoryManager.GetAll(), "Id", "Name", product.CategoryId);
            return View(product);
        }

        public ActionResult Edit(int? Id)
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = _productManager.Find(i => i.Id == Id.Value);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.Categories = new SelectList(_categoryManager.GetAll(), "Id", "Name", product.CategoryId);
            return View(product);
        }

        [HttpPost]
        public ActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                //db.Entry(product).State = EntityState.Modified;
                //db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Categories = new SelectList(_categoryManager.GetAll(), "Id", "Name", product.CategoryId);
            return View(product);
        }

        public ActionResult Delete(int? Id)
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = _productManager.Find(i => i.Id == Id.Value);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        [HttpPost]
        public ActionResult Delete(int Id)
        {
            Product product = _productManager.Find(i => i.Id == Id); ;
            //db.Products.Remove(product);
            //db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
