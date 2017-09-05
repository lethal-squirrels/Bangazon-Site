using Bangazon.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Bangazon.Models.OrderViewModels
{
    public class OrderCompletedViewModel
    {
        public Order Order { get; set; }

        [DisplayFormat(DataFormatString = "{0:C}")]
        [Display(Name = "Order Total")]
        public double OrderTotal { get; set; }

        public OrderCompletedViewModel(ApplicationDbContext ctx, ApplicationUser user, int? orderid)
        {
            Order = ctx.Order.Where(o => orderid == o.OrderID).Include("LineItems.Product").Include("PaymentType").SingleOrDefault();
            OrderTotal = Order.LineItems.Sum(x => x.Product.Price);

        }
    }
}


