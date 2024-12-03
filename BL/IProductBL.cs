using CrudForCategory.Models;

namespace CrudForCategory.BL
{
    public interface IProductBL
    {
        Task<bool> CreateProductAsync(ProductMaster model);
        Task<bool> DeleteProductAsync(int id);
        ProductMaster Details(int id);
        Task<bool> EditProductAsync(ProductMaster product);
        Task<(List<ProductMaster>, int)> GetPagedProductsAsync(int page, int pageSize);
    }
}
