using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Bangazon.Data;
using Bangazon.Models;
using Bangazon.Models.ProductViewModels;

namespace Bangazon.Controllers
{
    public class ProductTypesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductTypesController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: ProductTypes
        public async Task<IActionResult> Index()
        {
            var categoryViewModel = new CategoryIndexViewModel();
            var productTypes = await _context.ProductType.ToListAsync();
            var products = await _context.Product.ToListAsync();

            categoryViewModel.productTypeCounts = (from t in _context.ProductType
                                                   join p in _context.Product
                                                   on t.ProductTypeID equals p.ProductTypeID
                                                   group new { t, p } by new { t.Label, t.ProductTypeID } into grouped
                                                   select new ProductCountViewModel
                                                   {
                                                       TypeName = grouped.Key.Label,
                                                       ProductCount = grouped.Select(x => x.p.ProductID).Count(),
                                                       ProductTypeID = grouped.Key.ProductTypeID,
                                                       Products = grouped.Select(x => x.p).Take(3)
                                                   }).ToList();

            foreach (var type in categoryViewModel.productTypeCounts)
            {
                categoryViewModel.Products = products.Where(p => p.ProductType.ProductTypeID == type.ProductTypeID).Take(3).ToList();
            }

            return View(categoryViewModel);
        }

        //Displays all products in a specific category
        // GET: ProductTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            //if not found display 404
            if (id == null)
            {
                return NotFound();
            }

            //creates a new instance of the viewmodel
            var productTypeViewModel = new ProductTypeDetailViewModel();

            //grabs all product type 
            var productType = await _context.ProductType
                .SingleOrDefaultAsync(m => m.ProductTypeID == id);
            if (productType == null)
            {
                return NotFound();
            }


            productTypeViewModel.Products = await _context.Product
                            .Where(p =>p.ProductTypeID == id)
                            .ToListAsync();

            productTypeViewModel.ProductType = productType;


            return View(productTypeViewModel);
        }

        // GET: ProductTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ProductTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductTypeID,Label,Quantity")] ProductType productType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(productType);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(productType);
        }

        // GET: ProductTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productType = await _context.ProductType.SingleOrDefaultAsync(m => m.ProductTypeID == id);
            if (productType == null)
            {
                return NotFound();
            }
            return View(productType);
        }

        // POST: ProductTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductTypeID,Label,Quantity")] ProductType productType)
        {
            if (id != productType.ProductTypeID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductTypeExists(productType.ProductTypeID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(productType);
        }

        // GET: ProductTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productType = await _context.ProductType
                .SingleOrDefaultAsync(m => m.ProductTypeID == id);
            if (productType == null)
            {
                return NotFound();
            }

            return View(productType);
        }

        // POST: ProductTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productType = await _context.ProductType.SingleOrDefaultAsync(m => m.ProductTypeID == id);
            _context.ProductType.Remove(productType);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool ProductTypeExists(int id)
        {
            return _context.ProductType.Any(e => e.ProductTypeID == id);
        }
    }
}
