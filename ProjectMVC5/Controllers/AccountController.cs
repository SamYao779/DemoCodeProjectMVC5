using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using ProjectMVC5.Models;
using System.Data.Entity;
using System.IO;
using System.Web.Security;

namespace ProjectMVC5.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        public AccountController()
            : this(new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext())))
        {
        }

        public AccountController(UserManager<ApplicationUser> userManager)
        {
            UserManager = userManager;
        }

        public UserManager<ApplicationUser> UserManager { get; private set; }

        [AllowAnonymous]
        public ActionResult EditProfile()
        {
            using (var db = new ProjectMvcDbContext())
            {
                var model = db.Customers.Find(User.Identity.Name);
                return PartialView("_EditProfile", model);
            }
        }

        [HttpPost, AllowAnonymous]
        public ActionResult EditProfile(Customer model, HttpPostedFileBase upAnh)
        {
            
            if (upAnh != null && upAnh.ContentLength > 0)
            {
                if (System.IO.File.Exists(Server.MapPath("~/Images/Customers/" + model.Photo)))
                {
                    System.IO.File.Delete(Server.MapPath("~/Images/Customers/" + model.Photo));
                }
                var photoName = model.Id + upAnh.FileName.Substring(upAnh.FileName.LastIndexOf("."));
                string path = Path.Combine(Server.MapPath("~/Images/Customers") + Path.GetFileName(photoName));
                upAnh.SaveAs(path);
                model.Photo = photoName;
            }
            if (ModelState.IsValid)
            {
                using (var db = new ProjectMvcDbContext())
                {
                    try
                    {
                        db.Entry(model).State = EntityState.Modified;
                        db.SaveChanges();
                        ViewBag.Message = "Success";
                        ModelState.AddModelError("", "Cập nhật Profile thành công");
                        return View("Error");
                    }
                    catch (Exception Ex)
                    {
                        ViewBag.Message = "Error";
                        ModelState.AddModelError("", Ex.Message);
                        return View("Error");
                    }
                }
            }
            ViewBag.Message = "Error";
            return View("Error");
        }


        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return PartialView("_ForgotPassword");
            //return View();
        }

        [HttpPost, AllowAnonymous]
        public ActionResult ForgotPassword(string UserName, string Email)
        {
            var user = UserManager.FindByName(UserName);
            if (user != null)
            {
                using (var db = new ProjectMvcDbContext())
                {
                    var custom = db.Customers.Find(UserName);
                    if (custom.Email == Email)
                    {
                        var newcode = Guid.NewGuid().ToString();
                        UserManager.RemovePassword(user.Id);
                        UserManager.AddPassword(user.Id, newcode);

                        var to = Email;
                        var subject = "Reset Password";
                        var body = "Sử dụng chuỗi ký tự này để thay đổi mật khẩu " + newcode;

                        XMail.Send(to, subject, body);
                    }
                    else
                    {
                        ViewBag.Message = "Error";
                        ModelState.AddModelError("", "Email không hợp lệ");
                        return View("Error");
                    }
                }
            }
            else
            {
                ViewBag.Message = "Error";
                ModelState.AddModelError("", "Sai tên đăng nhập");
                return View("Error");
            }
            ViewBag.Message = "Success";
            ModelState.AddModelError("", "Hãy kiểm tra Email để biết mật khẩu mới. Đăng nhập để thay đổi mật khẩu");
            return View("Error");
        }

        [AllowAnonymous]
        public ActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost, AllowAnonymous]
        public ActionResult ResetPassword(string UserName, string NewCode, string NewPass, string ConfirmPass)
        {
            if (NewPass == ConfirmPass)
            {
                var user = UserManager.FindByName(UserName);
                if (user != null)
                {
                    var newpass = UserManager.ChangePassword(user.Id, NewCode, NewPass);
                    if (newpass.Succeeded)
                    {
                        return RedirectToAction("Login");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Sai chuỗi xác nhận");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Sai tên đăng nhập");
                }
            }
            else
            {
                ModelState.AddModelError("", "Mật khẩu không khớp");
            }
            return View();
        }

        [AllowAnonymous]
        public ActionResult Activate(string Id)
        {
            using (var db = new ProjectMvcDbContext())
            {
                var active = db.Customers.Find(Id.FromBase64());
                if (active != null)
                {
                    active.Activated = true;
                    db.SaveChanges();

                    ViewBag.Message = "Success";
                    ModelState.AddModelError("", "Congratulation ! your account is activated !");
                    return View("Error");
                }
            }
            return RedirectToAction("Register");
        }


        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            //return View();
            return PartialView("_Login");
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindAsync(model.UserName, model.Password);
                if (user != null)
                {
                    using (var db = new ProjectMvcDbContext())
                    {
                        var custom = db.Customers.Find(model.UserName);
                        if (custom.Activated)
                        {
                            await SignInAsync(user, model.RememberMe);
                            // Stay on same page
                            return Redirect(Request.UrlReferrer.ToString());
                        }
                        else
                        {
                            ViewBag.Message = "Error";
                            ModelState.AddModelError("", "Tài khoản chưa kích hoạt !");
                            return View("Error");
                        }
                    }
                }
                else
                {
                    ViewBag.Message = "Error";
                    ModelState.AddModelError("", "Sai username or password.");
                    return View("Error");
                }
            }

            // If we got this far, something failed, redisplay form
            ViewBag.Message = "Error";
            return View("Error");
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return PartialView("_Register");
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(Customer model, string ConfirmPassword, HttpPostedFileBase upAnh)
        {
            //var photo = Request.Files["upAnh"];
            //if (photo.ContentLength > 0)
            //{
            //    var photoName = model.Id + photo.FileName.Substring(photo.FileName.LastIndexOf("."));
            //    photo.SaveAs(Server.MapPath("~/Images/Customers/" + photoName));
            //    model.Photo = photoName;
            //}
            if (upAnh != null && upAnh.ContentLength > 0)
            {
                var photoName = model.Id + upAnh.FileName.Substring(upAnh.FileName.LastIndexOf("."));
                string path = Path.Combine(Server.MapPath("~/Images/Customers"), Path.GetFileName(photoName));
                upAnh.SaveAs(path);
                model.Photo = photoName;
            }
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser() { UserName = model.Id };
                if(model.Password != null)
                {
                    var result = await UserManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        using (var db = new ProjectMvcDbContext())
                        {
                            db.Customers.Add(model);
                            db.SaveChanges();
                        }

                        //var to = model.Email;
                        //var subject = "Welcome to ........";
                        //var url = Request.Url.AbsoluteUri.Replace("Register", "Activate/" + XString.ToBase64(model.Id.ToString()));
                        //var body = "Vui lòng nhấp vào liên kết sau để kích hoạt tài khoản <a href='" + url + "'>Activate</a>";
                        //XMail.Send(to, subject, body);

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ViewBag.Message = "Error";
                        ModelState.AddModelError("", "Register Fail");
                        return View("Error");
                    }
                }
                else
                {
                    ViewBag.Message = "Error";
                    ModelState.AddModelError("", "Password cannot null");
                    return View("Error");
                }
            }
            // If we got this far, something failed, redisplay form
            ViewBag.Message = "Error";
            return View("Error");
        }

        //
        // POST: /Account/Disassociate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Disassociate(string loginProvider, string providerKey)
        {
            ManageMessageId? message = null;
            IdentityResult result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }
            return RedirectToAction("Manage", new { Message = message });
        }

        //
        // GET: /Account/Manage
        public ActionResult Manage(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : message == ManageMessageId.Error ? "An error has occurred."
                : "";
            ViewBag.HasLocalPassword = HasPassword();
            ViewBag.ReturnUrl = Url.Action("Manage");
            return View();
        }

        //
        // POST: /Account/Manage
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Manage(ManageUserViewModel model)
        {
            bool hasPassword = HasPassword();
            ViewBag.HasLocalPassword = hasPassword;
            ViewBag.ReturnUrl = Url.Action("Manage");
            if (hasPassword)
            {
                if (ModelState.IsValid)
                {
                    IdentityResult result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
            }
            else
            {
                // User does not have a password so remove any validation errors caused by a missing OldPassword field
                ModelState state = ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }

                if (ModelState.IsValid)
                {
                    IdentityResult result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var user = await UserManager.FindAsync(loginInfo.Login);
            if (user != null)
            {
                await SignInAsync(user, isPersistent: false);
                return RedirectToLocal(returnUrl);
            }
            else
            {
                // If the user does not have an account, then prompt the user to create an account
                ViewBag.ReturnUrl = returnUrl;
                ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { UserName = loginInfo.DefaultUserName });
            }
        }

        //
        // POST: /Account/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // Request a redirect to the external login provider to link a login for the current user
            return new ChallengeResult(provider, Url.Action("LinkLoginCallback", "Account"), User.Identity.GetUserId());
        }

        //
        // GET: /Account/LinkLoginCallback
        public async Task<ActionResult> LinkLoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
            if (loginInfo == null)
            {
                return RedirectToAction("Manage", new { Message = ManageMessageId.Error });
            }
            var result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
            if (result.Succeeded)
            {
                return RedirectToAction("Manage");
            }
            return RedirectToAction("Manage", new { Message = ManageMessageId.Error });
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser() { UserName = model.UserName };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInAsync(user, isPersistent: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            //return RedirectToAction("Index", "Home");
            return Redirect(Request.UrlReferrer.ToString());
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult RemoveAccountList()
        {
            var linkedAccounts = UserManager.GetLogins(User.Identity.GetUserId());
            ViewBag.ShowRemoveButton = HasPassword() || linkedAccounts.Count > 1;
            return (ActionResult)PartialView("_RemoveAccountPartial", linkedAccounts);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && UserManager != null)
            {
                UserManager.Dispose();
                UserManager = null;
            }
            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private async Task SignInAsync(ApplicationUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            var identity = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            Error
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        private class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties() { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}