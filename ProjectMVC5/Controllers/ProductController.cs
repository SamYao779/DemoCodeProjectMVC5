using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectMVC5.Controllers
{
    public class ProductController : Controller
    {
        ProjectMvcDbContext db = new ProjectMvcDbContext();
        //
        // GET: /Product/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ProductList(string Id, int pageNo = 0)
        {
            List<Product> model = null;
            if (Id == "High")
            {
                ViewBag.Title = "Sản phẩm cao cấp";
                model = db.Products
                    .Where(p => p.Price > 5000)
                    .ToList();
            }
            else if (Id == "Medium")
            {
                ViewBag.Title = "Sản phẩm tầm trung";
                model = db.Products
                    .Where(p => p.Price > 1000 && p.Price <= 5000)
                    .ToList();
            }
            else if (Id == "Low")
            {
                ViewBag.Title = "Sản phẩm phổ thông";
                model = db.Products
                    .Where(p => p.Price <= 1000)
                    .ToList();
            }
            else if (Id == "Male")
            {
                ViewBag.title = "Đồng hồ nam";
                model = db.Products
                    .Where(p => p.Gender == "nam")
                    .ToList();
            }
            else if (Id == "Female")
            {
                ViewBag.title = "Đồng hồ nữ";
                model = db.Products
                    .Where(p => p.Gender == "nữ")
                    .ToList();
            }
            else if (Id != null)
            {
                model = db.Products
                    .Where(p => p.CategoryId.ToString() == Id)
                    .ToList();
            }
            if (Request.IsAjaxRequest())
            {
                return PartialView("_ProductList", model.Skip(pageNo * 9).Take(9));
            }
            ViewBag.Count = Math.Ceiling(1.0 * model.Count / 9);
            return View(model.Skip(pageNo * 9).Take(9));
        }

        public ActionResult Search(string ProdNo, int pageNo = 0)
        {
            ViewBag.Title = "Kết quả tìm kiếm " + ProdNo;
            var model = db.Products.Where(p => p.ProductNo.Contains(ProdNo) || p.Price.ToString().Contains(ProdNo)).ToList();
            if (Request.IsAjaxRequest())
            {
                return PartialView("_ProductList", model.Skip(pageNo * 9).Take(9));
            }
            ViewBag.Count = Math.Ceiling(1.0 * model.Count / 9);
            return View(model.Skip(pageNo * 9).Take(9));
        }

        public ActionResult ProductUser(int pageNo = 0)
        {
            ViewBag.Title = "Purchased Products";
            var model = db.OrderDetails
                    .Where(p => p.Order.CustomerId == User.Identity.Name)
                    .Select(p => p.Product).ToList();
            if (Request.IsAjaxRequest())
            {
                return PartialView("_ProductList", model.Skip(pageNo * 9).Take(9));
            }
            ViewBag.Count = Math.Ceiling(1.0 * model.Count / 9);
            return View(model.Skip(pageNo * 9).Take(9));
        }

        public ActionResult Detail(int Id)
        {
            var model = db.Products.Find(Id);

            var view = Request.Cookies["Views"];
            if (view == null)
            {
                view = new HttpCookie("Views", Id.ToString());
            }
            if (!view.Value.Contains(Id.ToString()))
            {
                view.Value += ", " + Id;
            }
            view.Expires = DateTime.Now.AddDays(1);
            Response.Cookies.Add(view);

            var ids = view.Value.Split(',')
                .Select(id => int.Parse(id)).ToList();
            ViewBag.view = db.Products.Where(p => ids.Contains(p.Id));
            return View(model);
        }
    }
}