using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Bangazon.Models
{
    public class Product
    {
        [Key]
        public int ProductID { get; set; }

        [Required]
        [StringLength(55)]
        public string Name { get; set; }

        [Required]
        [StringLength(255)]
        public string Description { get; set; }

        public int Quantity { get; set; }

        [Required]
        [DataType(DataType.Date)]
      
        public DateTime DateCreated { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public double Price { get; set; }

        [Required]
        public virtual ApplicationUser User { get; set; }

        [Required]
        public int ProductTypeID { get; set; }

        [Display(Name = "Category")]
        public ProductType ProductType { get; set; }

        [StringLength(30)]
        public string Location { get; set; }

        public string ImgPath { get; set; }

        [Display(Name ="Line Items")]
        public virtual ICollection<ProductOrder> LineItems { get; set; }

       
    }
}
