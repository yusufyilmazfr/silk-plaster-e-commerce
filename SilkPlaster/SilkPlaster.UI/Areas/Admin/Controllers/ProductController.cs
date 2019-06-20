﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SilkPlaster.BusinessLayer;
using SilkPlaster.BusinessLayer.Result;
using SilkPlaster.Entities;
using SilkPlaster.UI.Models;
using SilkPlaster.UI.Models.Helpers;
using SilkPlaster.UI.Models.Helpers.Image;

namespace SilkPlaster.UI.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        ProductManager _productManager = new ProductManager();
        CategoryManager _categoryManager = new CategoryManager();

        public ActionResult Index()
        {
            var products = _productManager.ListQueryable().Include(p => p.Category).OrderByDescending(i => i.AddedDate);
            return View(products.ToList());
        }

        public ActionResult Create()
        {
            ViewBag.Categories = new SelectList(_categoryManager.GetAll(), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(Product product, HttpPostedFileBase file, IEnumerable<HttpPostedFileBase> otherFiles)
        {
            ModelState.Remove("FirstImage");
            ModelState.Remove("AddedDate");

            if (ModelState.IsValid)
            {
                product.LongDescription = HttpUtility.HtmlEncode(product.LongDescription);

                ImageUploadResultMessage message = ImageHelper.Save(file, Server.MapPath("~/images/products/"));

                if (message.Result)
                {
                    product.FirstImage = message.FileName;

                    if (otherFiles.Count() > 0)
                    {
                        List<ProductImage> productImages = new List<ProductImage>();

                        foreach (var otherFile in otherFiles)
                        {
                            message = ImageHelper.Save(otherFile, Server.MapPath("~/images/products/"));

                            if (message.Result)
                            {
                                productImages.Add(new ProductImage
                                {
                                    Name = message.FileName,
                                });
                            }
                        }
                        product.ProductImages = productImages;
                    }


                    BusinessLayerResult<Product> layerResult = _productManager.Insert(product);

                    if (layerResult.Errors.Count == 0)
                    {
                        return RedirectToAction("Index");
                    }
                }
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

            Product product = _productManager
                .ListQueryable()
                .Include("ProductImages")
                .FirstOrDefault(i => i.Id == Id.Value);

            if (product == null)
            {
                return HttpNotFound();
            }

            product.LongDescription = HttpUtility.HtmlDecode(product.LongDescription);
            ViewBag.Categories = new SelectList(_categoryManager.GetAll(), "Id", "Name", product.CategoryId);
            return View(product);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(Product product, HttpPostedFileBase file, IEnumerable<HttpPostedFileBase> otherFiles)
        {
            ModelState.Remove("FirstImage");
            ModelState.Remove("AddedDate");

            if (ModelState.IsValid)
            {
                product.LongDescription = HttpUtility.HtmlEncode(product.LongDescription);

                ImageUploadResultMessage message = ImageHelper.Save(file, Server.MapPath("~"));

                if (message.Result)
                {
                    ImageHelper.Remove(Server.MapPath("~/images/products/") + product.FirstImage);

                    product.FirstImage = message.FileName;
                }

                if (otherFiles.Count() > 0)
                {
                    List<ProductImage> productImages = new List<ProductImage>();

                    foreach (var otherFile in otherFiles)
                    {
                        message = ImageHelper.Save(otherFile, Server.MapPath("~/images/products/"));

                        if (message.Result)
                        {
                            productImages.Add(new ProductImage
                            {
                                Name = message.FileName,
                            });
                        }
                    }
                    if (productImages.Count > 0)
                    {
                        product.ProductImages = productImages;
                    }
                }


                BusinessLayerResult<Product> layerResult = _productManager.Update(product);

                if (layerResult.Errors.Count == 0)
                {
                    return RedirectToAction("Index");
                }
                layerResult.Errors.ForEach(x => ModelState.AddModelError("", x.ErrorMessage));
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
            Product product = _productManager
                .ListQueryable()
                .Include("ProductImages")
                .FirstOrDefault(i => i.Id == Id);

            List<string> images = product.ProductImages.Select(i => i.Name).ToList();
            images.Add(product.FirstImage);

            int count = _productManager.Delete(product);

            if (count > 0)
            {
                string absolutePath = Server.MapPath("~/images/products/");

                foreach (var item in images)
                {
                    ImageHelper.Remove(absolutePath + item);
                }
            }

            return RedirectToAction("Index");
        }

    }
}
