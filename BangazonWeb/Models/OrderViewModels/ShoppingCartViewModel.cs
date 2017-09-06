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
    public class ShoppingCartViewModel
    {
        public ApplicationUser User { get; set; }

        public Order Order { get; set; }

        public IEnumerable<Product> Products { get; set; }

        public int ProductsCount { get; set; }

        public ShoppingCartViewModel() { }

        public ShoppingCartViewModel(ApplicationDbContext _context, ApplicationUser user, Order currentOrder)
        {
            Order = currentOrder;
            User = user;
            List<Product> products = new List<Product>();
            var allProducts = _context.Product.ToList();
            var productOrders = _context.ProductOrder.Where(po => po.OrderID == Order.OrderID).ToList();

            var productsInOrder = (from p in allProducts 
                                   join po in productOrders on p.ProductID equals po.ProductID
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
                    ProductsCount += p.LineItems.Count();
                    products.Add(newProduct);
                    break;
                }
            }
            Products = products;
        }
    }
}
