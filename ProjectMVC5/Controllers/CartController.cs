using ProjectMVC5.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectMVC5.Controllers
{
    public class CartController : Controller
    {
        //
        // GET: /Cart/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddNow(int Id)
        {
            ShoppingCart.Cart.Add(Id);
            return RedirectToAction("Index");
        }

        public ActionResult Add(int Id)
        {
            ShoppingCart.Cart.Add(Id);
            var info = new
            {
                ShoppingCart.Cart.Count,
                ShoppingCart.Cart.Amout
            };
            return Json(info, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Update()
        {
            foreach (var p in ShoppingCart.Cart.Items)
            {
                var Id = p.Id.ToString();
                p.Quantity = int.Parse(Request[Id]);
            }
            return RedirectToAction("Index");
        }

        public ActionResult Remove(int Id)
        {
            ShoppingCart.Cart.Remove(Id);
            var info = new
            {
                ShoppingCart.Cart.Count,
                amout = ShoppingCart.Cart.Amout.ToString("c")
            };
            return Json(info, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Clear()
        {
            ShoppingCart.Cart.clear();
            return RedirectToAction("Index");
        }
	}
}