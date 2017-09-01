using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bangazon.Models;
using Bangazon.Data;
using Bangazon.Models.ProductViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System;
using System.IO;

namespace Bangazon.Controllers
{
    public class ProductsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        private ApplicationDbContext _context;
        public ProductsController(ApplicationDbContext ctx, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _context = ctx;
        }
       

        // This task retrieves the currently authenticated user
        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        public async Task<IActionResult> Index()
        {
            // Create new instance of the view model
            ProductListViewModel model = new ProductListViewModel();

            // Set the properties of the view model
            model.Products = await _context.Product.ToListAsync();
            return View(model);
        }

        //POST: Products/Search
        [HttpPost]
        public async Task<IActionResult> Search(ProductSearchViewModel model)
        {
            model.Products = await _context.Product.ToListAsync();
            if (model.SearchRadio == "products")
            {
              
                if (model.Products != null)
                {
                    model.Products = (from product in model.Products
                                  where product.Name.ToLower().Contains(model.SearchTerms.ToLower())
                                  select product);

                    return View(model);
                }
                else
                {
                    return View("NoProductsFound");
                }

              
            }
            else if (model.SearchRadio == "location")
            {
                if (model.Products != null)
                {
                    model.Products = (from product in model.Products
                                  where product.Location == model.SearchTerms
                                  select product);

                    return View(model);
                }
                else
                {
                    return View("NoProductsFound");
                }
            }
            else
            {
                return View("NoProductsFound");
            }
        }


        // GET: Products/Details/5

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            // If no id was in the route, return 404
            if (id == null)
            {
                return NotFound();
            }

            // Create new instance of view model
            ProductDetailViewModel model = new ProductDetailViewModel();

            // Set the `Product` property of the view model
            model.Product = await _context.Product
                    .Include(prod => prod.User)
                    .SingleOrDefaultAsync(prod => prod.ProductID == id);
            // If product not found, return 404
            if (model.Product == null)
            {
                return NotFound();
            }
            return View(model);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Create()
        {
            ProductCreateViewModel model = new ProductCreateViewModel(_context);

            // Get current user
            var user = await GetCurrentUserAsync();

            return View(model);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductCreateViewModel viewModel)
        {
            // Remove the user from the model validation because it is
            // not information posted in the form
       
            ModelState.Remove("Product.User");

            if (ModelState.IsValid)
            {
                /*
                    If all other properties validate, then grab the 
                    currently authenticated user and assign it to the 
                    product before adding it to the db _context
                */
                var user = await GetCurrentUserAsync();
                viewModel.Product.User = user;
                viewModel.Product.DateCreated = DateTime.Now;
                

                if (viewModel.ProductPhoto != null)
                {
                    if (viewModel.ProductPhoto.Length > 0)
                    {
                        string directory = Directory.GetCurrentDirectory();
                        string localSavePath = directory + @"\wwwroot\images\" + viewModel.ProductPhoto.FileName;
                        string dbPath = "/images/" + viewModel.ProductPhoto.FileName;
                        using (var stream = new FileStream(localSavePath, FileMode.Create))
                        {
                            await viewModel.ProductPhoto.CopyToAsync(stream);
                        }
                        viewModel.Product.ImgPath = dbPath;
                    }
                }
                

                _context.Add(viewModel.Product);

                await _context.SaveChangesAsync();
                var routeID = viewModel.Product.ProductID;
                return RedirectToAction("Details", "Products", new { @id = routeID });

            }
            ProductCreateViewModel newmodel = new ProductCreateViewModel(_context);
            return View(newmodel);
        }

        // GET: Products/Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producttobedeleted = await _context.Product
                .Include(p => p.LineItems.Select(li => li.Order).Where(o => o.PaymentTypeID == null))
                .SingleOrDefaultAsync(p => p.ProductID == id);

            if (producttobedeleted == null)
            {
                return View(producttobedeleted);
            }

            return View(producttobedeleted);

        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var prod = await _context.Product.SingleOrDefaultAsync(m => m.ProductID == id);
            _context.Product.Remove(prod);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Types()
        {
            var model = new ProductTypesViewModel();

            // Get line items grouped by product id, including count
            var counter = from product in _context.Product
                          group product by product.ProductTypeID into grouped
                          select new { grouped.Key, myCount = grouped.Count() };

            // Build list of Product instances for display in view
            model.ProductTypes = await (from type in _context.ProductType
                                        join a in counter on type.ProductTypeID equals a.Key
                                        select new ProductType
                                        {
                                            ProductTypeID = type.ProductTypeID,
                                            Label = type.Label,
                                            Quantity = a.myCount
                                        }).ToListAsync();

            return View(model);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
