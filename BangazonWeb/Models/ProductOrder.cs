using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Bangazon.Models
{
    public class ProductOrder
    {
        [Key]
        public int ProductOrderID { get; set; }

        [Required]
        public int OrderID { get; set; }

        [Required]
        public int ProductID { get; set; }

        public virtual Order Order { get; set; }

        public virtual Product Product { get; set; }
    }
}
