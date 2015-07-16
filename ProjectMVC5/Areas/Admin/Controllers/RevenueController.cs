using ProjectMVC5.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectMVC5.Areas.Admin.Controllers
{
    [Authorize]
    public class RevenueController : Controller
    {
        ProjectMvcDbContext db = new ProjectMvcDbContext();
        //
        // GET: /Admin/Revenue/
        public ActionResult ByProduct()
        {
            ViewBag.Message = "Revenue by Product";
            var model = db.OrderDetails.GroupBy(p => p.Product)
                .Select(g => new Report
                {
                    Group = g.Key.ProductNo,
                    Amount = g.Sum(p => p.Quantity * p.UnitPrice),
                    Count = g.Sum(p => p.Quantity),
                    Max = g.Max(p => p.UnitPrice),
                    Min = g.Min(p => p.UnitPrice),
                    Avg = g.Average(p => p.UnitPrice)
                });
            return View("Revenue", model);
        }

        public ActionResult ByCategory()
        {
            ViewBag.Message = "Revenue by Category";
            var model = db.OrderDetails.GroupBy(p => p.Product.Category)
                .Select(g => new Report
                {
                    Group = g.Key.Name,
                    Amount = g.Sum(p => p.Quantity * p.UnitPrice),
                    Count = g.Sum(p => p.Quantity),
                    Max = g.Max(p => p.UnitPrice),
                    Min = g.Min(p => p.UnitPrice),
                    Avg = g.Average(p => p.UnitPrice)
                });
            return View("Revenue", model);
        }

        public ActionResult ByCustomer()
        {
            ViewBag.Message = "Revenue by Customer";
            var model = db.OrderDetails.GroupBy(p => p.Order.Customer)
                .Select(g => new Report
                {
                    Group = g.Key.Id,
                    Amount = g.Sum(p => p.Quantity * p.UnitPrice),
                    Count = g.Sum(p => p.Quantity),
                    Max = g.Max(p => p.UnitPrice),
                    Min = g.Min(p => p.UnitPrice),
                    Avg = g.Average(p => p.UnitPrice)
                });
            return View("Revenue", model);
        }

        public ActionResult ByYear()
        {
            ViewBag.Message = "Revenue for Year";
            var model = db.OrderDetails.GroupBy(p => p.Order.OrderDate.Year)
                .Select(g => new Report
                {
                    IGroup = g.Key,
                    Amount = g.Sum(p => p.Quantity * p.UnitPrice),
                    Count = g.Sum(p => p.Quantity),
                    Max = g.Max(p => p.UnitPrice),
                    Min = g.Min(p => p.UnitPrice),
                    Avg = g.Average(p => p.UnitPrice)
                })
                .OrderByDescending(p=>p.IGroup);
            return View("Revenue", model);
        }

        public ActionResult ByMonth()
        {
            ViewBag.Message = "Revenue for Month";
            var model = db.OrderDetails.GroupBy(p => p.Order.OrderDate.Month)
                .Select(g => new Report
                {
                    IGroup = g.Key,
                    Amount = g.Sum(p => p.Quantity * p.UnitPrice),
                    Count = g.Sum(p => p.Quantity),
                    Max = g.Max(p => p.UnitPrice),
                    Min = g.Min(p => p.UnitPrice),
                    Avg = g.Average(p => p.UnitPrice)
                })
                .OrderByDescending(p => p.IGroup);
            return View("Revenue", model);
        }

        public ActionResult ByQuarter()
        {
            ViewBag.Message = "Revenue for Quarter";
            var model = db.OrderDetails.GroupBy(p =>(int)Math.Ceiling(1.0 * p.Order.OrderDate.Month / 3))
                .Select(g => new Report
                {
                    IGroup = g.Key,
                    Amount = g.Sum(p => p.Quantity * p.UnitPrice),
                    Count = g.Sum(p => p.Quantity),
                    Max = g.Max(p => p.UnitPrice),
                    Min = g.Min(p => p.UnitPrice),
                    Avg = g.Average(p => p.UnitPrice)
                })
                .OrderBy(p => p.IGroup);
            return View("Revenue", model);
        }
    }
}