using deneme.Data;     // ApplicationDbContext
using deneme.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace deneme.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;     // ← doğru tip

        public HomeController(ILogger<HomeController> logger,
                              ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var viewModel = new HomeViewModel
            {
                //
                FlashSaleProducts = _context.Products.Where(p => p.IsFlashSale).ToList(),
                PopularProducts = _context.Products.Where(p => p.IsPopular).ToList()
            };

            return View(viewModel);
        }

        public IActionResult Privacy() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}
