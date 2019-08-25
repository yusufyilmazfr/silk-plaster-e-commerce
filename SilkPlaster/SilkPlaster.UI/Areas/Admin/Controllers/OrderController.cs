using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SilkPlaster.BusinessLayer.Abstract;
using SilkPlaster.BusinessLayer.Concrete.Result;
using SilkPlaster.Entities;
using SilkPlaster.Entities.Concrete;
using SilkPlaster.UI.Models;
using SilkPlaster.UI.Models.Filters;

namespace SilkPlaster.UI.Areas.Admin.Controllers
{
    [AdminAuthFilter]
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
        public ActionResult Edit(Order currentOrder)
        {
            if (ModelState.IsValid)
            {
                BusinessLayerResult<Order> layerResult =
                    _orderManager.ChangeTrackingNumberAndState(currentOrder.Id, currentOrder.CargoTrackingNumber, currentOrder.OrderState);

                if (!layerResult.HasError())
                    return RedirectToAction("Index");

                layerResult.Errors.ForEach(x => ModelState.AddModelError("", x.ErrorMessage));

            }
            return View(currentOrder);
        }

    }
}
