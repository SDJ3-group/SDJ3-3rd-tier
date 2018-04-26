using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SDJ3_3rd_tier.Models
{
    public class Package
    {
        public int Id { get; set; }
        public bool Repacking { get; set; }

        public virtual ICollection<Part> Parts { get; set; }
    }
}