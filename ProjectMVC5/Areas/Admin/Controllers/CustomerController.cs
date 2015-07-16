using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using ProjectMVC5.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectMVC5.Areas.Admin.Controllers
{
    [Authorize]
    public class CustomerController : Controller
    {
        ProjectMvcDbContext db = new ProjectMvcDbContext();
        //
        // GET: /Admin/Customer/
        public ActionResult Index()
        {
            var model = db.Customers.ToList();
            return View(model);
        }

        public ActionResult ConfirmDelete(string Id)
        {
            var model = db.Customers.Find(Id);
            return PartialView("_Delete", model);
        }

        [HttpPost]
        public ActionResult Delete(string Id)
        {
            var model = db.Customers.Find(Id);
            if (System.IO.File.Exists(Server.MapPath("~/Images/Customers/" + model.Photo)))
            {
                System.IO.File.Delete(Server.MapPath("~/Images/Customers/" + model.Photo));
            }
            db.Customers.Remove(model);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
	}
}