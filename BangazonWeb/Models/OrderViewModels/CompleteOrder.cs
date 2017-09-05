using Bangazon.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Bangazon.Models.OrderViewModels
{
    public class CompleteOrder
    {
        public List<SelectListItem> PaymentTypeId { get; set; }

        public ApplicationUser User { get; set; }

        public Order Order { get; set; }

        [DisplayFormat(DataFormatString = "{0:C}")]
        [Display(Name = "Order Total")]
        public double OrderTotal { get; set; }

        public CompleteOrder(){}

        public CompleteOrder(ApplicationDbContext ctx, ApplicationUser user)
        {
            User = user;

            Order = ctx.Order.Where(o => o.PaymentType == null && o.User.Id == user.Id).Include("LineItems.Product").SingleOrDefault(); ;

            OrderTotal = Order.LineItems.Sum(x => x.Product.Price);

            this.PaymentTypeId = ctx.PaymentType
                        .Where(pt => pt.User == user && pt.IsActive == true)
                        .OrderBy(l => l.Description)
                        .AsEnumerable()
                        .Select(li => new SelectListItem
                        {
                            Text = li.Description,
                            Value = li.PaymentTypeID.ToString()
                        }).ToList();

            this.PaymentTypeId.Insert(0, new SelectListItem
            {
                Text = "Choose Payment Type...",
                Value = "0"
            });
        }
    }
}
