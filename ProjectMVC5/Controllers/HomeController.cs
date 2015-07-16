using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectMVC5.Controllers
{
    public class HomeController : Controller
    {
        ProjectMvcDbContext db = new ProjectMvcDbContext();
        public ActionResult Index()
        {
            ViewBag.High = db.Products.Where(p => p.Price > 5000)
                .Select(x => new ProductViewModel
                {
                    Id = x.Id,
                    ProductNo = x.ProductNo,
                    Image = x.Image,
                    Price = x.Price
                })
                .OrderBy(p => Guid.NewGuid())
                .Take(6)
                .ToList();
            ViewBag.Medium = db.Products.Where(p => p.Price > 1000 && p.Price <= 5000)
                .Select(x => new ProductViewModel
                {
                    Id = x.Id,
                    ProductNo = x.ProductNo,
                    Image = x.Image,
                    Price = x.Price
                })
                .OrderBy(p => Guid.NewGuid())
                .Take(6)
                .ToList();
            ViewBag.Low = db.Products.Where(p => p.Price <= 1000)
                .Select(x => new ProductViewModel
                {
                    Id = x.Id,
                    ProductNo = x.ProductNo,
                    Image = x.Image,
                    Price = x.Price
                })
                .OrderBy(p => Guid.NewGuid())
                .Take(6)
                .ToList();
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult CategoryMenu()
        {
            var model = db.Categories.OrderBy(p => p.Name);
            return PartialView("_Category", model);
        }
    }
}