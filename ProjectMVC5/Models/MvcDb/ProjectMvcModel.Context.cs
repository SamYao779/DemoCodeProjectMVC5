
namespace ProjectMVC5
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;

    public class ProjectMvcDbContext : DbContext
    {
        public ProjectMvcDbContext()
            : base("name=ProjectMVC")
        {
        }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<OrderDetail> OrderDetails { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderState> OrderStates { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Permission> Permissions { get; set; }
        public virtual DbSet<WebAction> WebActions { get; set; }
    }
}
