using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ProjectMVC5.Areas.Admin.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        ProjectMvcDbContext db = new ProjectMvcDbContext();
        //
        // GET: /Admin/Order/
        public ActionResult Index()
        {
            var model = db.Orders.ToList();
            return View(model);
        }
       
        public ActionResult Edit(int Id)
        {
            var model = db.Orders.Find(Id);
            ViewBag.OrderState = db.OrderStates.ToList();
            return PartialView("_Edit", model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Update(Order model)
        {
            try
            {
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch(Exception Ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, Ex.Message);
            }
        }

        public ActionResult ConfirmDelete(int Id)
        {
            var model = db.Orders.Find(Id);
            return PartialView("_Delete", model);
        }
        [HttpPost]
        public ActionResult Delete(int Id)
        {
            var model = db.Orders.Find(Id);
            db.Orders.Remove(model);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult ViewDetail(int Id)
        {
            var order = db.Orders.Find(Id);
            var model = db.OrderDetails.Where(p => p.OrderId == order.Id).ToList();
            return PartialView("_Detail", model);
        }
	}
}