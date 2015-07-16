using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ProjectMVC5.Areas.Admin.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        ProjectMvcDbContext db = new ProjectMvcDbContext();
        //
        // GET: /Admin/Product/
        public ActionResult Index(int pageNo = 0)
        {
            var model = db.Products.ToList();
            if(Request.IsAjaxRequest())
            {
               return PartialView("_Product", model.Skip(pageNo * 10).Take(10));
            }
            ViewBag.pageCount = Math.Ceiling(1.0 * model.Count / 10);
            return View(model.Skip(pageNo * 10).Take(10));
        }

        public ActionResult Add()
        {
            ViewBag.Category = db.Categories.ToList();
            return PartialView("_Add");
        }

        [HttpPost, ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Add(Product model, HttpPostedFileBase upImage)
        {
            if (upImage != null && upImage.ContentLength > 0)
            {
                string path = Path.Combine(Server.MapPath("~/Images/Products"), Path.GetFileName(upImage.FileName));
                upImage.SaveAs(path);
                model.Image = upImage.FileName;
            }
            try
            {
                db.Products.Add(model);
                db.SaveChanges();
                return Redirect("Index");
            }
            catch (Exception Ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, Ex.Message);
            }
        }

        public ActionResult Edit(int Id)
        {
            var model = db.Products.Find(Id);
            ViewBag.Category = db.Categories.ToList();
            return PartialView("_Edit", model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Update(Product model, HttpPostedFileBase upImage)
        {
            if (upImage != null && upImage.ContentLength > 0)
            {
                if (System.IO.File.Exists(Server.MapPath("~/Images/Products/" + model.Image)))
                {
                    System.IO.File.Delete(Server.MapPath("~/Images/Products/" + model.Image));
                }
                string path = Path.Combine(Server.MapPath("~/Images/Products"), Path.GetFileName(upImage.FileName));
                upImage.SaveAs(path);
                model.Image = upImage.FileName;
            }
            try
            {
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return Redirect("Index");
            }
            catch
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Fail to Update Items");
            }

        }

        public ActionResult ConfirmDelete(int Id)
        {
            var model = db.Products.Find(Id);
            return PartialView("_Delete", model);
        }

        public ActionResult Delete(int Id)
        {
            var model = db.Products.Find(Id);
            if (System.IO.File.Exists(Server.MapPath("~/Images/Products/" + model.Image)))
            {
                System.IO.File.Delete(Server.MapPath("~/Images/Products/" + model.Image));
            }
            db.Products.Remove(model);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}