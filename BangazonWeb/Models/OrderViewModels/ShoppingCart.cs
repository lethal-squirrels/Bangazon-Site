using Bangazon.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections;
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

        public ShoppingCart(ApplicationDbContext _context, ApplicationUser user)
        {
            User = user;
            List<Product> products = new List<Product>();
            var allProducts = _context.Product.ToList();
            var curentOrder = _context.Order.SingleOrDefault(o => o.PaymentType == null && o.User.Id == user.Id);
            var productOrders = _context.ProductOrder.Where(po => po.OrderID == curentOrder.OrderID).ToList();

            var productsInOrder = (from p in _context.Product 
                                   join po in _context.ProductOrder.Where(po => po.OrderID == curentOrder.OrderID)
                                   on p.ProductID equals po.ProductID
                                   select p
                                   ).ToList();
            var groupedProducts = productsInOrder.GroupBy(p => p.ProductID);
            foreach (var product in groupedProducts)
            {
                foreach(var p in product)
                {
                    Product newProduct = new Product
                    {
                        ProductID = p.ProductID,
                        Name = p.Name,
                        Description = p.Description,
                        Quantity = p.LineItems.Count(),
                        DateCreated = p.DateCreated,
                        Price = p.Price * p.LineItems.Count(),
                        User = p.User,
                        ProductTypeID = p.ProductTypeID
                    };
                    products.Add(newProduct);
                    break;
                }
            }
            Products = products;
        }
    }
}
