
namespace ProjectMVC5
{
    using System;
    using System.Collections.Generic;

    public class Product
    {
        public int Id { get; set; }
        public string ProductNo { get; set; }
        public int CategoryId { get; set; }
        public string Gender { get; set; }
        public string Movement { get; set; }
        public string CaseMaterial { get; set; }
        public string BandMaterial { get; set; }
        public string Image { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public string Warranty { get; set; }

        public virtual Category Category { get; set; }
        public virtual List<OrderDetail> OrderDetails { get; set; }
    }
}
