using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SDJ3_3rd_tier.Models.DTOs
{
    public class PackageParts
    {
        public virtual ICollection<int> Parts { get; set; }
    }
}