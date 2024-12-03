using CrudForCategory.Data;
using CrudForCategory.Models;
using CrudForCategory.Request;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CrudForCategory.BL
{
    public class CategoryBL : ICategoryBL
    {
        private readonly AppDbContext _context;
        public CategoryBL(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }

        public async Task<List<CategoryMaster>> Index()
        {
            var categories = await _context.CategoryMasters
                                           .Include(c => c.ProductMasters)
                                           .ToListAsync();
            return categories;
        }

        public async Task<CategoryMaster> Details(int? id)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id), "Category ID cannot be null.");

            var category = await _context.CategoryMasters
                                         .Include(c => c.ProductMasters)
                                         .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
                throw new KeyNotFoundException($"Category with ID {id} was not found.");

            return category;
        }

        public async Task<CategoryMaster> Create(CategoryViewRequest model)
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
            return category;
        }

        public CategoryUpadateViewResult Edit(CategoryMaster category)
        {


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

            return model;
        }

        public async Task<CategoryMaster> Edit(CategoryMaster existingCategory, CategoryUpadateViewResult model)
        {

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

            return existingCategory;
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var category = await _context.CategoryMasters.FindAsync(id);

            if (category == null)
            {
                return false; 
            }

            _context.CategoryMasters.Remove(category);
            await _context.SaveChangesAsync();

            return true; 
        }

    }
}
