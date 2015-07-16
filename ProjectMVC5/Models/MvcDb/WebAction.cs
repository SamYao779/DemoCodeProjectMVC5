using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectMVC5
{
    public class WebAction
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual List<Permission> Permissions { get; set; }
    }
}