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
    public class UserController : Controller
    {
        public ApplicationDbContext db = new ApplicationDbContext();
        public UserManager<ApplicationUser> UserManager { get; private set; }

        public UserController()
        {
            UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
        }
        //
        // GET: /Admin/Uer/
        public ActionResult Index()
        {
            ViewBag.User = db.Users;
            var model = new ApplicationUser();
            return View(model);
        }

        public ActionResult Add(ApplicationUser model, string NewPass, string ConfirmNewPass)
        {
            if (NewPass != ConfirmNewPass)
            {
                ModelState.AddModelError("", "Mật khẩu không khớp !");
            }
            else
            {
                var result = UserManager.Create(model, NewPass);
                if (result.Succeeded)
                {
                    ModelState.AddModelError("", "Thêm mới thành công !");
                }
                else
                {
                    ModelState.AddModelError("", "Thêm mới thất bại !");
                }
            }
            ViewBag.User = db.Users;
            return View("Index", model);
        }

        public ActionResult Update(ApplicationUser model, string OldPass, string NewPass, string ConfirmNewPass)
        {
            if (NewPass != ConfirmNewPass)
            {
                ModelState.AddModelError("", "Mật khẩu không khớp !");
            }
            else
            {
                var user = UserManager.Find(model.UserName, OldPass);
                if (user != null)
                {
                    UserManager.ChangePassword(user.Id, OldPass, NewPass);
                }
                else
                {
                    ModelState.AddModelError("", "Sai mật khẩu !");
                }
            }
            ViewBag.User = db.Users;
            return View("Index", model);
        }

        public ActionResult Edit(string Id)
        {
            var model = db.Users.Find(Id);
            ViewBag.User = db.Users;
            return View("Index", model);
        }

        public ActionResult Delete(string Id)
        {
            ApplicationUser model = db.Users.Find(Id);
            db.Users.Remove(model);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}