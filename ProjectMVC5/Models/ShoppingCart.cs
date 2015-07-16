using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectMVC5.Models
{
    public class ShoppingCart
    {
        public static ShoppingCart Cart
        {
            get
            {
                var cart = HttpContext.Current.Session["Cart"] as ShoppingCart;
                if (cart == null)
                {
                    cart = new ShoppingCart();
                    HttpContext.Current.Session["Cart"] = cart;
                }
                return cart;
            }
        }

        public List<Product> Items = new List<Product>();

        public int Count
        {
            get
            {
                if (Items.Count > 0)
                {
                    return Items.Sum(p=>p.Quantity);
                }
                return 0;
            }
        }

        public double Amout
        {
            get
            {
                if (Items.Count > 0)
                {
                    return Items.Sum(p => p.Price * p.Quantity);
                }
                return 0;
            }
        }

        public void Add(int Id)
        {
            try
            {
                var prod = Items.Single(p => p.Id == Id);
                prod.Quantity++;
            }
            catch
            {
                using (var db = new ProjectMvcDbContext())
                {
                    var prod = db.Products.Find(Id);
                    prod.Quantity = 1;
                    Items.Add(prod);
                }
            }
        }

        public void Remove(int Id)
        {
            var prod = Items.Single(p => p.Id == Id);
            Items.Remove(prod);
        }

        public void Update(int Id, int NewQuantity)
        {
            var prod = Items.Single(p => p.Id == Id);
            prod.Quantity = NewQuantity;
        }

        public void clear()
        {
            Items.Clear();
        }
    }
}