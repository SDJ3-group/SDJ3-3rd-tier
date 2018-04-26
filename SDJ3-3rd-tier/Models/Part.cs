using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SDJ3_3rd_tier.Models
{
    public class Part
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public Double Weight { get; set; }

        public virtual Car Car { get; set; }
        public virtual Pallet Pallet { get; set; }
        public virtual Package Package { get; set; }
    }
}