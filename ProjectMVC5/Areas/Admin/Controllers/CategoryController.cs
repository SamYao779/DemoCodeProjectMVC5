using ProjectMVC5;
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
    public class CategoryController : Controller
    {
        ProjectMvcDbContext db = new ProjectMvcDbContext();        
        //
        // GET: /Admin/Category/
        public ActionResult Index()
        {
            var model = db.Categories.ToList();
            return View(model);
        }

        public ActionResult Add()
        {
            return PartialView("_Add");
        }
        [HttpPost, ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Add(Category model)
        {
            try
            {
                db.Categories.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception Ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, Ex.Message);
            }

            
        }

        public ActionResult Edit(int Id)
        {
            var model = db.Categories.Find(Id);
            return PartialView("_Edit", model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Update(Category model)
        {
            try
            {
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception Ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, Ex.Message);
            }
        }

        public ActionResult ConfirmDelete(int Id)
        {
            var model = db.Categories.Find(Id);
            return PartialView("_Delete", model);
        }
        [HttpPost]
        public ActionResult Delete(int Id)
        {
            var model = db.Categories.Find(Id);
            db.Categories.Remove(model);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}