using ProjectMVC5.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectMVC5.Areas.Admin.Controllers
{
    [Authorize]
    public class InventoryController : Controller
    {
        ProjectMvcDbContext db = new ProjectMvcDbContext();
        //
        // GET: /Admin/Inventory/
        public ActionResult Index()
        {
            ViewBag.Message = "Inventory by Category";
            var detail = db.Products.GroupBy(p => p.Category)
                .Select(g => new Report
                {
                    Group = g.Key.Name,
                    Amount = g.Sum(p=>p.Quantity * p.Price),
                    Count = g.Sum(p=>p.Quantity),
                    Min = g.Min(p=>p.Price),
                    Max = g.Max(p=>p.Price),
                    Avg = g.Average(p=>p.Price)
                });
            return View("Inventory", detail);
        }
	}
}