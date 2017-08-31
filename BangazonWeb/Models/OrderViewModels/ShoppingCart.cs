using Bangazon.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bangazon.Models.OrderViewModels
{
    public class ShoppingCart
    {
        public ApplicationUser User { get; set; }

        public Order Order { get; set; }

        public IEnumerable<Product> Products { get; set; }

        public ShoppingCart(ApplicationDbContext _context)
        {
            
            var products = _context.Product.ToList();
            var curentOrder = _context.Order.SingleOrDefault(o => o.PaymentType == null);
            var productOrders = _context.ProductOrder.Where(po => po.OrderID == curentOrder.OrderID).ToList();

            Products = (from p in products 
                        join po in productOrders on p.ProductID equals po.ProductID
                        select p
                        );

            
        }
    }
}
