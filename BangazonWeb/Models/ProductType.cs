﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bangazon.Models
{
    public class ProductType
    {
        [Key]
        public int ProductTypeID { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name = "Category")]
        public string Label { get; set; }

        public int Quantity { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}