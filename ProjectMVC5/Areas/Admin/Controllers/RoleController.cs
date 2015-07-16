using Microsoft.AspNet.Identity.EntityFramework;
using ProjectMVC5.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectMVC5.Areas.Admin.Controllers
{
    public class RoleController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        //
        // GET: /Admin/Role/
        public ActionResult Index()
        {
            ViewBag.Role = db.Roles;
            var model = new IdentityRole();
            return View(model);
        }

        public ActionResult Add(IdentityRole model)
        {
            try
            {
                db.Roles.Add(model);
                db.SaveChanges();
                var Projectdb = new ProjectMvcDbContext();
                foreach (var p in Projectdb.WebActions)
                {
                    var perm = new Permission
                    {
                        Allow = false,
                        RoleId = model.Id,
                        WebActionId = p.Id
                    };
                    Projectdb.Permissions.Add(perm);
                }
                
                Projectdb.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception Ex)
            {
                ModelState.AddModelError("", Ex.Message);
                ViewBag.Role = db.Roles;
                return View("Index", model);
            }
        }

        public ActionResult Update(IdentityRole model)
        {
            try
            {
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                ModelState.AddModelError("", "Cập nhật thành công !");
            }
            catch (Exception Ex)
            {
                ModelState.AddModelError("", Ex.Message);
            }
            ViewBag.Role = db.Roles;
            return View("Index", model);
        }

        public ActionResult Edit(string Id)
        {
            var model = db.Roles.Find(Id);
            ViewBag.Role = db.Roles;
            return View("Index", model);
        }

        public ActionResult Remove(string Id)
        {
            var model = db.Roles.Find(Id);
            db.Roles.Remove(model);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}