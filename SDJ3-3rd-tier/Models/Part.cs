using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SDJ3_3rd_tier.Models
{
    public class Part
    {
        [Key, Required]
        public int Id { get; set; }
        [Required]
        public String Name { get; set; }
        [Required]
        public Double Weight { get; set; }

        public virtual Car Car { get; set; }
        public virtual Pallet Pallet { get; set; }
        public virtual Package Package { get; set; }
    }
}