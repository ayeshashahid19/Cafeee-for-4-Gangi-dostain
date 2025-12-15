using CafeManagementSystem.Data;
using CafeManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CafeManagementSystem.Controllers
{
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;
        public OrderController(ApplicationDbContext context) => _context = context;

        public IActionResult Index()
        {
            var orders = _context.Orders.OrderByDescending(o => o.OrderDate).ToList();
            return View(orders);
        }

        public IActionResult Create()
        {
            ViewBag.Products = new SelectList(_context.Products, "ProductId", "ProductName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(List<OrderItem> items)
        {
            if (items == null || items.Count == 0)
            {
                ModelState.AddModelError("", "Add at least one product");
                ViewBag.Products = new SelectList(_context.Products, "ProductId", "ProductName");
                return View();
            }

            decimal total = 0;
            foreach (var i in items)
            {
                var p = _context.Products.Find(i.ProductId);
                i.Product = p;
                total += p.Price * i.Quantity;
            }

            var order = new Order
            {
                OrderDate = DateTime.Now,
                TotalAmount = total
            };

            _context.Orders.Add(order);
            _context.SaveChanges();

            foreach (var i in items)
            {
                i.OrderId = order.OrderId;
                _context.OrderItems.Add(i);
            }
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Details(int id)
        {
            var order = _context.Orders.Find(id);
            if (order == null) return NotFound();

            var items = _context.OrderItems.Where(oi => oi.OrderId == id)
                                           .Select(oi => new {
                                               oi.Product.ProductName,
                                               oi.Quantity,
                                               Price = oi.Product.Price,
                                               Total = oi.Quantity * oi.Product.Price
                                           }).ToList();

            ViewBag.OrderItems = items;
            return View(order);
        }
    }
}
