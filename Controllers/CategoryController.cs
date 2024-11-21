using CrudForCategory.Data;
using CrudForCategory.Models;
using CrudForCategory.Request;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CrudForCategory.Controllers
{
	public class CategoryController : Controller
	{
		private readonly AppDbContext _context;

		public CategoryController(AppDbContext context)
		{
			_context = context;
		}

		public async Task<IActionResult> Index()
		{
			var categories = await _context.CategoryMasters
										   .Include(c => c.ProductMasters)
										   .ToListAsync();
			return View(categories);
		}

		public async Task<IActionResult> Details(int? id)
		{
			if (id == null) return NotFound();

			var category = await _context.CategoryMasters
										 .Include(c => c.ProductMasters)
										 .FirstOrDefaultAsync(m => m.Id == id);
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
				var category = new CategoryMaster
				{
					CategoryName = model.CategoryName,
					ProductMasters = model.ProductNames
						.Where(name => !string.IsNullOrWhiteSpace(name.ToString()))
						.Select(name => new ProductMaster { ProductName = name.ToString() })
						.ToList()
				};

				_context.CategoryMasters.Add(category);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}

			return View(model);
		}


		public async Task<IActionResult> Edit(int id)
		{
			var category = await _context.CategoryMasters
				.Include(c => c.ProductMasters)
				.FirstOrDefaultAsync(c => c.Id == id);

			if (category == null)
				return NotFound();

			var model = new CategoryUpadateViewResult
			{
				Id = category.Id,
				CategoryName = category.CategoryName,
				ProductNames = category.ProductMasters.Select(p => new ProductUpdateViewRequest
				{
					Id = p.Id,
					ProductName = p.ProductName
				}).ToList()
			};

			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(CategoryUpadateViewResult model)
		{
			if (!ModelState.IsValid)
				return View(model);

			var existingCategory = await _context.CategoryMasters
				.Include(c => c.ProductMasters)
				.FirstOrDefaultAsync(c => c.Id == model.Id);

			if (existingCategory == null)
				throw new Exception("Category not found");

		
			existingCategory.CategoryName = model.CategoryName;

		
			var existingProductIds = existingCategory.ProductMasters.Select(p => p.Id).ToList();
			var updatedProductIds = model.ProductNames.Select(p => p.Id).ToList();

		
			var productsToRemove = existingCategory.ProductMasters
				.Where(p => !updatedProductIds.Contains(p.Id))
				.ToList();

			_context.ProductMasters.RemoveRange(productsToRemove);

		
			foreach (var product in model.ProductNames)
			{
				if (product.Id == 0)
				{
				
					existingCategory.ProductMasters.Add(new ProductMaster
					{
						ProductName = product.ProductName,
						CategoryId = existingCategory.Id
					});
				}
				else
				{
				
					var existingProduct = existingCategory.ProductMasters.FirstOrDefault(p => p.Id == product.Id);
					if (existingProduct != null)
					{
						existingProduct.ProductName = product.ProductName;
					}
				}
			}

		
			await _context.SaveChangesAsync();

			return RedirectToAction(nameof(Index));
		}

		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null) return NotFound();

			var category = await _context.CategoryMasters
										 .FirstOrDefaultAsync(m => m.Id == id);
			if (category == null) return NotFound();

			return View(category);
		}

		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var category = await _context.CategoryMasters.FindAsync(id);
			if (category != null)
			{
				_context.CategoryMasters.Remove(category);
				await _context.SaveChangesAsync();
			}
			return RedirectToAction(nameof(Index));
		}
	}
}
