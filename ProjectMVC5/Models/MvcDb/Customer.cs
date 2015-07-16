
namespace ProjectMVC5
{
    using System;
    using System.Collections.Generic;
    
    public class Customer
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public int IdGroup { get; set; }
        public bool Activated { get; set; }
        public string Photo { get; set; }
    
        public virtual List<Order> Orders { get; set; }
    }
}
