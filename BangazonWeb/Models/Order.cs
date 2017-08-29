using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Bangazon.Models
{
    public class Order
    {
        [Key]
        public int OrderID { get; set; }

        [Required]
        public virtual ApplicationUser User { get; set; }

        public int? PaymentTypeID { get; set; }

        public virtual PaymentType PaymentType { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime DateCreated { get; set; }

        [Display(Name = "Line Items")]
        public ICollection<ProductOrder> LineItems { get; set; }

    }
}
