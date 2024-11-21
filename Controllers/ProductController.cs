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

        
        public IActionResult Details(int id)
        {
            var product = _context.ProductMasters
                .Include(p => p.Category)  
                .FirstOrDefault(p => p.Id == id);  

            if (product == null)
            {
                return NotFound();  
            }

            return View(product);  
        }



        public IActionResult Create()
        {
            ViewBag.Categories = _context.CategoryMasters.ToList();
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductMaster model)
        {

            _context.ProductMasters.Add(model);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Product created successfully!";
            return RedirectToAction(nameof(Index));

        }


        public IActionResult Edit(int id)
        {
            var product = _context.ProductMasters.Include(p => p.Category)
                                                 .FirstOrDefault(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            ViewBag.Categories = _context.CategoryMasters.ToList();
            return View(product);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductMaster product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            try
            {
                _context.Update(product);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Product updated successfully!";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.ProductMasters.Any(p => p.Id == product.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }

        
        public IActionResult Delete(int id)
        {
            var product = _context.ProductMasters.Include(p => p.Category)
                                                 .FirstOrDefault(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.ProductMasters.FindAsync(id);
            if (product != null)
            {
                _context.ProductMasters.Remove(product);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Product deleted successfully!";
            }
            return RedirectToAction(nameof(Index));
        }


    }
}
