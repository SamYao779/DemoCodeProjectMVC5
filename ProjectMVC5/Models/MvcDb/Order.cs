
namespace ProjectMVC5
{
    using System;
    using System.Collections.Generic;
    
    public class Order
    {
        public int Id { get; set; }
        public string CustomerId { get; set; }
        public string Receiver { get; set; }
        public System.DateTime OrderDate { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public int OrderStateId { get; set; }
        public double Amount { get; set; }
        public string Description { get; set; }
    
        public virtual Customer Customer { get; set; }
        public virtual List<OrderDetail> OrderDetails { get; set; }
        public virtual OrderState OrderState { get; set; }
    }
}
