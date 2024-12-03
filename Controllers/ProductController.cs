using CrudForCategory.BL;
using CrudForCategory.Data;
using CrudForCategory.Models;
using CrudForCategory.Request;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CrudForCategory.Controllers
{
	public class ProductController : Controller
	{
		private readonly IProductBL _context;
		private readonly ICategoryBL _categoryBL;
		public ProductController(IProductBL product, ICategoryBL categoryBL)
		{
			_context = product;
			_categoryBL = categoryBL;
		}
		public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
		{
			var (products, totalProducts) = await _context.GetPagedProductsAsync(page, pageSize);

			ViewBag.TotalPages = (int)Math.Ceiling((double)totalProducts / pageSize);
			ViewBag.CurrentPage = page;

			return View(products);
		}



		public IActionResult Details(int id)
		{
			var product = _context.Details(id);

			if (product == null)
			{
				return NotFound();
			}

			return View(product);
		}



		public IActionResult Create()
		{
			var categories = _categoryBL.Index().Result;
			ViewBag.Categories = categories;

			return View();
		}




		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(ProductMaster model)
		{

			var success = await _context.CreateProductAsync(model);

			if (success)
			{
				TempData["SuccessMessage"] = "Product created successfully!";
				return RedirectToAction(nameof(Index));
			}
			else
			{
				TempData["ErrorMessage"] = "An error occurred while creating the product.";
				return View(model);
			}


			var categories = _categoryBL.Index().Result;
			ViewBag.Categories = categories;
			return View(model);
		}



		public IActionResult Edit(int id)
		{
			var product = _context.Details(id);

			if (product == null)
			{
				return NotFound();
			}

			ViewBag.Categories = _categoryBL.Index().Result;
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

			//if (ModelState.IsValid)
			//{
			var success = await _context.EditProductAsync(product);

			if (success)
			{
				TempData["SuccessMessage"] = "Product updated successfully!";
				return RedirectToAction(nameof(Index));
			}
			else
			{
				TempData["ErrorMessage"] = "An error occurred while updating the product.";
				return View(product);
			}
			//}

			var categories = _categoryBL.Index().Result;
			ViewBag.Categories = categories;

			return View(product);
		}



		public IActionResult Delete(int id)
		{
			var product = _context.Details(id);

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
			var success = await _context.DeleteProductAsync(id);

			if (success)
			{
				TempData["SuccessMessage"] = "Product deleted successfully!";
			}
			else
			{
				TempData["ErrorMessage"] = "Product not found, deletion failed.";
			}

			return RedirectToAction(nameof(Index)); // Redirect to Index after the operation
		}



	}
}
