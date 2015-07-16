using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectMVC5.Areas.Admin.Models
{
    public class Report
    {
        public String Group { get; set; }
        public int IGroup
        {
            set
            {
                Group = value.ToString();
            }
            get
            {
                return int.Parse(Group);
            }
        }
        public double Amount { get; set; }
        public int Count { get; set; }
        public double Min { get; set; }
        public double Max { get; set; }
        public double Avg { get; set; }
    }
}