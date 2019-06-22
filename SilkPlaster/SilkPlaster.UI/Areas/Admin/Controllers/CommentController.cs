using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SilkPlaster.BusinessLayer;
using SilkPlaster.Entities;
using SilkPlaster.UI.Models;
using SilkPlaster.UI.Models.Filters;

namespace SilkPlaster.UI.Areas.Admin.Controllers
{
    public class CommentController : Controller
    {
        CommentManager _commentManager = new CommentManager();

        public ActionResult Index()
        {
            var comments = _commentManager
                .ListQueryable()
                .Include("Product")
                .Include("Member")
                .OrderByDescending(i => i.AddedDate)
                .ToList();

            return View(comments);
        }

        public ActionResult Edit(int? Id)
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = _commentManager.Find(i => i.Id == Id.Value);

            if (comment == null)
            {
                return HttpNotFound();
            }

            return View(comment);
        }

        [HttpPost]
        public ActionResult Edit(Comment comment)
        {
            if (ModelState.IsValid)
            {
                //db.Entry(comment).State = EntityState.Modified;
                //db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(comment);
        }

        public ActionResult Delete(int? Id)
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = _commentManager.Find(i => i.Id == Id.Value);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        [HttpPost]
        public ActionResult Delete(int Id)
        {
            Comment comment = _commentManager.Find(i => i.Id == Id);
            _commentManager.Delete(comment);

            return RedirectToAction("Index");
        }
    }
}
