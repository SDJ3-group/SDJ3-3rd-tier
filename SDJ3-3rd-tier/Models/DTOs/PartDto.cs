using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SDJ3_3rd_tier.Models.DTOs
{
    public class PartDto
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public String Name { get; set; }
        [Required]
        public Double Weight { get; set; }
        public String CarId { get; set; }
        public int? PalletId { get; set; }
        public int? PreviusPalletId { get; set; }
        public int? PackageId { get; set; }
    }
}