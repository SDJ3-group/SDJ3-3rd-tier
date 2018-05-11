using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SDJ3_3rd_tier.Models
{
    public class Pallet
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public const double MAX_WEIGHT = 12345;

        public virtual ICollection<Part> Parts { get; set; }
    }
}