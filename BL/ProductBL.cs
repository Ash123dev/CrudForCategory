using CrudForCategory.Data;
using CrudForCategory.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CrudForCategory.BL
{
    public class ProductBL : IProductBL
    {
        private readonly AppDbContext _context;
        public ProductBL(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }
        public async Task<(List<ProductMaster>, int)> GetPagedProductsAsync(int page, int pageSize)
        {
            var totalProducts = await _context.ProductMasters.CountAsync();

            var products = await _context.ProductMasters.Include(x => x.Category)
                                                         .OrderBy(x => x.Id)
                                                         .Skip((page - 1) * pageSize)
                                                         .Take(pageSize)
                                                         .ToListAsync();

            return (products, totalProducts);
        }



        public ProductMaster Details(int id)
        {
            var product = _context.ProductMasters
                .Include(p => p.Category)
                .FirstOrDefault(p => p.Id == id);

            if (product == null)
                throw new ArgumentNullException(nameof(id), "Category ID cannot be null.");

            return product;
        }

        public async Task<bool> CreateProductAsync(ProductMaster model)
        {
            _context.ProductMasters.Add(model);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> EditProductAsync(ProductMaster product)
        {
            try
            {

                var existingProduct = await _context.ProductMasters.FindAsync(product.Id);

                if (existingProduct == null)
                {
                    return false;
                }

                _context.Entry(existingProduct).CurrentValues.SetValues(product);

                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _context.ProductMasters.FindAsync(id);

            if (product == null)
            {
                return false; 
            }

            _context.ProductMasters.Remove(product);
            await _context.SaveChangesAsync();

            return true; 
        }

    }
}
