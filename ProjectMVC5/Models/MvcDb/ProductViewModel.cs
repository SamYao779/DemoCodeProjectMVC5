using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectMVC5
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string ProductNo { get; set; }
        public string Image { get; set; }
        public double Price { get; set; }
    }
}