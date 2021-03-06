﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SDJ3_3rd_tier.Models
{
    public class Package
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public bool Repacking { get; set; }
        [Required]
        public String Content { get; set; }


        public virtual ICollection<Part> Parts { get; set; }
    }
}