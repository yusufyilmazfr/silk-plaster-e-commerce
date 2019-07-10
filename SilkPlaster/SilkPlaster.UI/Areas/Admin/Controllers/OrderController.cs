using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SilkPlaster.BusinessLayer.Abstract;
using SilkPlaster.Entities;
using SilkPlaster.UI.Models;

namespace SilkPlaster.UI.Areas.Admin.Controllers
{
    public class OrderController : Controller
    {
        private IOrderManager _orderManager { get; set; }

        public OrderController(IOrderManager orderManager)
        {
            _orderManager = orderManager;
        }

        public ActionResult Index()
        {
            return View(_orderManager.GetAllOrdersWithDetails());
        }

        public ActionResult Details(int? Id)
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Order order = _orderManager.GetOrderDetailById(Id.Value);

            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        public ActionResult Edit(int? Id)
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Order order = _orderManager.GetOrderById(Id.Value);

            if (order == null)
            {
                return HttpNotFound();
            }

            return View(order);
        }

        [HttpPost]
        public ActionResult Edit([Bind(Include = "OrderState")] Order order)
        {
            if (ModelState.IsValid)
            {
                //db.Entry(order).State = EntityState.Modified;
                //db.SaveChanges();
                //return RedirectToAction("Index");
            }
            return View(order);
        }

    }
}
