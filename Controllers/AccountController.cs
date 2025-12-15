using CafeManagementSystem.Data;
using CafeManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace CafeManagementSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        public AccountController(ApplicationDbContext context) => _context = context;

        public IActionResult Login() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string username, string password)
        {
            var user = _context.Users
                .FirstOrDefault(u => u.Username == username && u.Password == password);

            if (user != null)
            {
                // Here you can implement session/cookie logic
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Invalid credentials");
            return View();
        }
    }
}
