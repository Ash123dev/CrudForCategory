using CrudForCategory.Data;
using CrudForCategory.Models;
using CrudForCategory.Request;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CrudForCategory.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        public ProductController(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
        {
            var totalProducts = await _context.ProductMasters.CountAsync();

            var products = await _context.ProductMasters.Include(x => x.Category)
                                                        .OrderBy(x => x.Id)
                                                        .Skip((page - 1) * pageSize)
                                                        .Take(pageSize)
                                                        .ToListAsync();

            ViewBag.TotalPages = (int)Math.Ceiling((double)totalProducts / pageSize);
            ViewBag.CurrentPage = page;

            return View(products);
        }
    }
}
