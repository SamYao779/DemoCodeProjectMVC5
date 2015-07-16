using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectMVC5
{
    public class Permission
    {
        public int Id { get; set; }
        public string RoleId { get; set; }
        public int WebActionId { get; set; }
        public Boolean Allow { get; set; }

        public virtual WebAction WebAction { get; set; }
        public virtual IdentityRole Role { get; set; }
    }
}