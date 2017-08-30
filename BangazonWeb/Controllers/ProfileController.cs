using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Bangazon.Data;
using Microsoft.AspNetCore.Identity;
using Bangazon.Models;
using Microsoft.EntityFrameworkCore;

namespace Bangazon.Controllers
{
    public class ProfileController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProfileController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        // This task retrieves the currently authenticated user
        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        // GET: ApplicationUser
        public async Task<IActionResult> Index()
        {
            ProfileViewModel model = new ProfileViewModel();

            model.User = await GetCurrentUserAsync();

            return View(model);
        }

        // GET: Profile/Edit
        public async Task<IActionResult> Edit()
        {

            ProfileViewModel model = new ProfileViewModel();

            model.User = await GetCurrentUserAsync();
            return View(model);
        }

        // POST: Profile/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProfileViewModel model)
        {
            var User = await GetCurrentUserAsync();
            User.FirstName = model.User.FirstName;
            User.LastName = model.User.LastName;
            User.StreetAddress = model.User.StreetAddress;
            User.City = model.User.City;
            User.State = model.User.State;
            User.ZipCode = model.User.ZipCode;
            User.Phone = model.User.Phone;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(User);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(User.Id))
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
            return View(model);
        }

        private bool UserExists(string id)
        {
            throw new NotImplementedException();
        }
    }
}