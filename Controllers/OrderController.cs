using CafeManagementSystem.Data;
using CafeManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CafeManagementSystem.Controllers
{
    [Authorize(Roles = "Cashier")]
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ===================== ORDER LIST =====================
        public IActionResult Index()
        {
            var orders = _context.Orders
                .OrderByDescending(o => o.OrderDate)
                .ToList();

            return View(orders);
        }

        // ===================== CREATE ORDER =====================
        public IActionResult Create()
        {
            ViewBag.Products = new SelectList(
                _context.Products,
                "ProductId",
                "ProductName");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(List<OrderItem> items)
        {
            if (items == null || !items.Any())
            {
                ModelState.AddModelError("", "Add at least one product");
                ViewBag.Products = new SelectList(_context.Products, "ProductId", "ProductName");
                return View();
            }

            decimal totalAmount = 0;

            // Calculate total
            foreach (var item in items)
            {
                var product = _context.Products.FirstOrDefault(p => p.ProductId == item.ProductId);
                if (product == null) continue;

                item.Price = product.Price;
                totalAmount += product.Price * item.Quantity;
            }

            // Create Order
            //Order order = new Order
            //{
            //    OrderDate = DateTime.Now,
            //    TotalAmount = totalAmount
            //};


            var order = new Order
            {
                OrderDate = DateTime.Now,
                TotalAmount = totalAmount,

                // 🔴 SAVE LOGGED-IN CASHIER
                CashierName = User.Identity.Name
            };

            _context.Orders.Add(order);
            _context.SaveChanges();

            // Save Order Items
            foreach (var item in items)
            {
                item.OrderId = order.OrderId;
                _context.OrderItems.Add(item);
            }

            _context.SaveChanges();

            // 🔴 Redirect to Receipt instead of Index
            return RedirectToAction("Receipt", new { id = order.OrderId });
        }

        // ===================== RECEIPT =====================
        public IActionResult Receipt(int id)
        {
            var order = _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefault(o => o.OrderId == id);

            if (order == null)
                return NotFound();

            return View(order);
        }

        // ===================== ORDER DETAILS =====================
        public IActionResult Details(int id)
        {
            var order = _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefault(o => o.OrderId == id);

            if (order == null)
                return NotFound();

            return View(order);
        }
    }
}
