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
using SilkPlaster.Entities.Concrete;
using SilkPlaster.UI.Models;
using SilkPlaster.UI.Models.Filters;

namespace SilkPlaster.UI.Areas.Admin.Controllers
{
    [AdminAuthFilter]
    public class CommentController : Controller
    {
        private ICommentManager _commentManager { get; set; }

        public CommentController(ICommentManager commentManager)
        {
            _commentManager = commentManager;
        }

        public ActionResult Index()
        {
            var comments = _commentManager.GetCommentsWithProductsAndMembers();
            return View(comments);
        }

        public ActionResult Edit(int? Id)
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = _commentManager.GetCommentWithMemberById(Id.Value);

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
                BusinessLayerResult<Comment> layerResult = _commentManager.UpdateComment(comment);

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

            Comment comment = _commentManager.GetCommentWithMemberById(Id.Value);

            if (comment == null)
            {
                return HttpNotFound();

            }
            return View(comment);
        }

        [HttpPost]
        public ActionResult Delete(int Id)
        {
            Comment comment = _commentManager.GetCommentById(Id);
            _commentManager.RemoveComment(comment);

            return RedirectToAction("Index");
        }
    }
}
