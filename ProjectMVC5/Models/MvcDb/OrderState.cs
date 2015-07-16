
namespace ProjectMVC5
{
    using System;
    using System.Collections.Generic;
    
    public class OrderState
    {
        public int Id { get; set; }
        public string Status { get; set; }
    
        public virtual List<Order> Orders { get; set; }
    }
}
