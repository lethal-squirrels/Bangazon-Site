using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Bangazon.Data;
using Bangazon.Models.HomeViewModels;
using Microsoft.EntityFrameworkCore;
using Bangazon.Models;

namespace Bangazon.Controllers
{
    public class HomeController : Controller
    {
        private  readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var homeview = new HomePageViewModel();

            homeview.Product = await _context.Product
                                .OrderByDescending(p => p.DateCreated)
                                .Take(20)
                                .ToListAsync();

            return View(homeview);
        }



        public IActionResult Error()
        {
            return View();
        }
    }
}
