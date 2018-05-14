using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SDJ3_3rd_tier.Models
{
    public class Part
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public String Name { get; set; }
        [Required]
        public Double Weight { get; set; }
        [Required]
        public String CarId { get; set; }
        public int? PalletId { get; set; }
        public int? PreviusPalletId { get; set; }
        public int? PackageId { get; set; }

        [ForeignKey("CarId"), Required]
        public virtual Car Car { get; set; }
        [ForeignKey("PalletId")]
        public virtual Pallet Pallet { get; set; }
        [ForeignKey("PreviusPalletId")]
        public virtual Pallet PreviousPallet  { get; set; }
        [ForeignKey("PackageId")]
        public virtual Package Package { get; set; }
    }
}