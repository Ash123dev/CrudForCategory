using CrudForCategory.BL;
using CrudForCategory.Data;
using CrudForCategory.Models;
using CrudForCategory.Request;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CrudForCategory.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryBL _context;

        public CategoryController(ICategoryBL context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _context.Index();
            return View(categories);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var category = await _context.Details(id);

            if (category == null) return NotFound();

            return View(category);
        }

        public IActionResult Create()
        {
            var model = new CategoryViewRequest();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryViewRequest model)
        {
            if (ModelState.IsValid)
            {
                var category = _context.Create(model);
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }


        public async Task<IActionResult> Edit(int id)
        {
            var category = await _context.Details(id);

            if (category == null)
                return NotFound();

            var model = _context.Edit(category);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CategoryUpadateViewResult model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var existingCategory = await _context.Details(model.Id);

            if (existingCategory == null)
                throw new Exception("Category not found");


            existingCategory.CategoryName = model.CategoryName;

            await _context.Edit(existingCategory, model);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var category = await _context.Details(id);
            if (category == null) return NotFound();

            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _context.DeleteCategoryAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
