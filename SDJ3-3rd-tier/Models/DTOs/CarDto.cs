﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SDJ3_3rd_tier.Models.DTOs
{
    public class CarDto
    {
        [Key, Required]
        public String VIN { get; set; }
        [Required]
        public String Model { get; set; }
        [Required]
        public Double Weight { get; set; }
    }
}