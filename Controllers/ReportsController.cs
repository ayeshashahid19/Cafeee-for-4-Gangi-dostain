using CafeManagementSystem.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CafeManagementSystem.Controllers
{
    [Authorize(Roles = "Admin")] // 🔒 ONLY ADMIN
    public class ReportsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReportsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult DailySales()
        {
            DateTime today = DateTime.Today;

            decimal total = _context.Orders
                .Where(o => o.OrderDate.Date == today)
                .Sum(o => (decimal?)o.TotalAmount) ?? 0;

            ViewBag.TotalSales = total;
            return View();
        }


        public IActionResult MonthlySales()
        {
            int month = DateTime.Now.Month;
            int year = DateTime.Now.Year;

            decimal total = _context.Orders
                .Where(o => o.OrderDate.Month == month && o.OrderDate.Year == year)
                .Sum(o => (decimal?)o.TotalAmount) ?? 0;

            ViewBag.TotalSales = total;
            return View();
        }

        public IActionResult BestSelling()
        {
            var data = _context.OrderItems
                .GroupBy(i => i.Product.ProductName)
                .Select(g => new
                {
                    Product = g.Key,
                    Quantity = g.Sum(x => x.Quantity)
                })
                .OrderByDescending(x => x.Quantity)
                .ToList();

            return View(data);
        }

        public IActionResult CashierSales()
        {
            var data = _context.Orders
                .GroupBy(o => o.CashierName)
                .Select(g => new
                {
                    Cashier = g.Key,
                    Total = g.Sum(x => x.TotalAmount)
                })
                .ToList();

            return View(data);
        }

        public IActionResult Profit()
        {
            decimal profit = _context.OrderItems
                .Sum(i => (i.Product.Price - i.Product.CostPrice) * i.Quantity);

            ViewBag.Profit = profit;
            return View();
        }




    }
}
