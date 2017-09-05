using Bangazon.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bangazon.Models.OrderViewModels
{
    public class OrderHistoryViewModel
    {
        public List<Order> Orders { get; set; }
        public OrderHistoryViewModel(ApplicationDbContext ctx, ApplicationUser user)
        {
            Orders = ctx.Order.Where(o => o.User.Id == user.Id && o.PaymentTypeID != null).ToList();
        }
    }
}
