
namespace ProjectMVC5
{
    using System;
    using System.Collections.Generic;
    
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Origin { get; set; }
    
        public virtual List<Product> Products { get; set; }
    }
}
