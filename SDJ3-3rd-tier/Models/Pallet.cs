using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SDJ3_3rd_tier.Models
{
    public class Pallet
    {
        public int Id { get; set; }
        public const double MAX_WEIGHT = 12345;

        public virtual ICollection<Part> Parts { get; set; }
    }
}