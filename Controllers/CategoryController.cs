using CafeManagementSystem.Data;
using CafeManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace CafeManagementSystem.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;
        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index() => View(_context.Categories.ToList());

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Categories.Add(category);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        public IActionResult Edit(int id)
        {
            var cat = _context.Categories.Find(id);
            return cat == null ? NotFound() : View(cat);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Categories.Update(category);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        public IActionResult Delete(int id)
        {
            var cat = _context.Categories.Find(id);
            return cat == null ? NotFound() : View(cat);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var cat = _context.Categories.Find(id);
            if (cat != null)
            {
                _context.Categories.Remove(cat);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
