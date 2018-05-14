using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SDJ3_3rd_tier.Models.DTOs
{
    public class PalletDto
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public double MaximumCapacity { get; set; }
    }
}