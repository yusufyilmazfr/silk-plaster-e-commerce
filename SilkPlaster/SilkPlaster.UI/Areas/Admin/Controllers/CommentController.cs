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
            Comment comment = _commentManager
                .ListQueryable()
                .Include("Member")
                .Where(i => i.Id == Id.Value)
                .FirstOrDefault();

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
                BusinessLayerResult<Comment> layerResult = _commentManager.Update(comment);

                if (layerResult.Errors.Count > 0)
                {
                    //comment.StarCount = comment.StarCount <= 5 && comment.StarCount >= 1 ? comment.StarCount : (byte)5;
                    layerResult.Errors.ForEach(x => ModelState.AddModelError("", x.ErrorMessage));
                    return View(comment);
                }

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

            Comment comment = _commentManager
                .ListQueryable()
                .Include("Member")
                .FirstOrDefault(i => i.Id == Id.Value);

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
