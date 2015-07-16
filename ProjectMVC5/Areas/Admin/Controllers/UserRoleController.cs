using Microsoft.AspNet.Identity.EntityFramework;
using ProjectMVC5.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectMVC5.Areas.Admin.Controllers
{
    public class UserRoleController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        //
        // GET: /Admin/UserRole/
        public ActionResult Index()
        {
            ViewBag.User = db.Users.ToList();
            ViewBag.Role = db.Roles;
            return View();
        }

        public ActionResult Update(string Role_Id, string User_Id)
        {
            var user = db.Users.Find(User_Id);
            try
            {
                var userRole = user.Roles.Single(p => p.RoleId == Role_Id);
                user.Roles.Remove(userRole);
                return Content("Remove");
            }
            catch
            {
                var userRole = new IdentityUserRole
                {
                    RoleId = Role_Id,
                    UserId = User_Id
                };
                user.Roles.Add(userRole);
                return Content("Add");
            }
            finally
            {
                db.SaveChanges();
            }
        }
	}
}