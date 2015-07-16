using ProjectMVC5.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectMVC5.Areas.Admin.Controllers
{
    public class PermissionController : Controller
    {
        ApplicationDbContext sdb = new ApplicationDbContext();
        ProjectMvcDbContext db = new ProjectMvcDbContext();
        //
        // GET: /Admin/Permission/
        public ActionResult Index()
        {
            ViewBag.Role = sdb.Roles;
            ViewBag.WebAction = db.WebActions;
            ViewBag.Permission = db.Permissions.ToList();
            return View();
        }

        public ActionResult Update(string Role_Id, int WebAction_Id)
        {
            var per = db.Permissions.Single(p => p.RoleId == Role_Id && p.WebActionId == WebAction_Id);
            per.Allow = !per.Allow;
            db.SaveChanges();
            return Content("");
        }
    }
}