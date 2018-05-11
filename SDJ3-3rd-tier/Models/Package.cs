using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SDJ3_3rd_tier.Models
{
    public class Package
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public bool Repacking { get; set; }

        public virtual ICollection<Part> Parts { get; set; }
    }
}