using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectMVC5.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        ProjectMvcDbContext db = new ProjectMvcDbContext();
        //
        // GET: /Order/
        public ActionResult OrderDetail(int Id)
        {
            var model = db.Orders.Find(Id);
            return PartialView("_OrderDetail", model);
        }

        public ActionResult Purchase(Order model)
        {
            try
            {
                db.Orders.Add(model);
                foreach (var p in ProjectMVC5.Models.ShoppingCart.Cart.Items)
                {
                    var detail = new OrderDetail()
                    {
                        Order = model,
                        ProductId = p.Id,
                        Quantity = p.Quantity,
                        UnitPrice = p.Price,
                    };
                    db.OrderDetails.Add(detail);
                }
                db.SaveChanges();
                ProjectMVC5.Models.ShoppingCart.Cart.clear();
                return RedirectToAction("OrderList");
            }
            catch
            {
                ModelState.AddModelError("", "Đặt hàng lỗi");
            }
            return View("Checkout");
        }

        public ActionResult OrderList()
        {
            var cust = db.Customers.Find(User.Identity.Name);
            var model = cust.Orders.OrderByDescending(p => p.OrderDate);
            return View(model);
        }

        public ActionResult Checkout()
        {
            var cust = db.Customers.Find(User.Identity.Name);
            var model = new Order();
            model.CustomerId = cust.Id;
            model.Receiver = cust.FullName;
            model.OrderDate = DateTime.Now;
            model.Amount = ProjectMVC5.Models.ShoppingCart.Cart.Amout;

            return View(model);
        }
    }
}