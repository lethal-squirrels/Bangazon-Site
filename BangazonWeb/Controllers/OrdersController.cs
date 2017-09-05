using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Bangazon.Data;
using Bangazon.Models;
using Microsoft.AspNetCore.Identity;
using Bangazon.Models.OrderViewModels;
using Microsoft.AspNetCore.Authorization;

namespace Bangazon.Controllers
{
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly UserManager<ApplicationUser> _userManager;

        public OrdersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        // GET: Orders/ShoppingCart
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> ShoppingCart()
        {
            var user = await GetCurrentUserAsync();
            var currentOrder = _context.Order.SingleOrDefault(o => o.PaymentType == null && o.User.Id == user.Id);
            if (currentOrder == null)
            {
                return View("ShoppingCartEmpty");
            }
     
            var shoppingCart = new ShoppingCart(_context, user, currentOrder);


            if (shoppingCart.Order == null)
            {
                return NotFound();
            }
            if (shoppingCart.Products.Count() < 1)
            {
                return View("ShoppingCartEmpty");
            }

            return View(shoppingCart);
        }

        // Creates a new Order
        public async Task<Order> CreateOrder()
        {
            var order = new Order();
            order.User = await GetCurrentUserAsync();
            order.DateCreated = DateTime.Now;

            _context.Add(order);
            await _context.SaveChangesAsync();
            return order;
        }

        //GET: Orders/EditCart
       
        public async Task<IActionResult> EditCart(int orderID, int ProductID)
        {
            if(orderID == 0 || ProductID == 0)
            {
                return View("ShoppingCartEmpty");
            }
            var prodOrder = await _context.ProductOrder
                            .FirstAsync(po => po.OrderID == orderID && po.ProductID == ProductID);


            _context.ProductOrder.Remove(prodOrder);
            await _context.SaveChangesAsync();



            return RedirectToAction("ShoppingCart");
            
        }

        // GET: Orders/Purchase/5
        [Authorize]
        public async Task<IActionResult> Purchase(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await GetCurrentUserAsync();
            var order = await _context.Order.SingleOrDefaultAsync(m => m.User.Id == user.Id && m.PaymentType == null);
            if (order == null)
            {
                order = await CreateOrder();
            }
            var product = await _context.Product.SingleOrDefaultAsync(p => p.ProductID == id);
            var productOrdersCount = await _context.ProductOrder.Where(po => po.ProductID == id && po.OrderID == order.OrderID).CountAsync();
            if (productOrdersCount >= product.Quantity)
            {
                return View("OutOfStock");
            }
            var productOrder = new ProductOrder();
            productOrder.Order = order;
            productOrder.Product = await _context.Product.SingleOrDefaultAsync(p => p.ProductID == id);
            _context.Add(productOrder);
            await _context.SaveChangesAsync();
            
            var purchase = new Purchase();
            purchase.Product = productOrder.Product;
            purchase.Order = order;

            return View(purchase);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = await GetCurrentUserAsync();
            var order = await _context.Order
                .SingleOrDefaultAsync(m => m.OrderID == id);
            if (order == null)
            {
                return NotFound();
            }
            var shoppingCart = new ShoppingCart(_context, user, order);

            return View(shoppingCart);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productOrders = await _context.ProductOrder.Where(po => po.OrderID == id).ToListAsync();
            foreach (var po in productOrders)
            {
                _context.ProductOrder.Remove(po);
            }
            var order = await _context.Order.SingleOrDefaultAsync(m => m.OrderID == id);
            _context.Order.Remove(order);
            await _context.SaveChangesAsync();
            return View("ShoppingCartEmpty");
        }

        private bool OrderExists(int id)
        {
            return _context.Order.Any(e => e.OrderID == id);
        }
    }
}
