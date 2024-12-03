using CrudForCategory.Models;
using CrudForCategory.Request;
using Microsoft.AspNetCore.Mvc;

namespace CrudForCategory.BL
{
    public interface ICategoryBL
    {
        Task<CategoryMaster> Create(CategoryViewRequest model);
        Task<bool> DeleteCategoryAsync(int id);
        Task<CategoryMaster> Details(int? id);
        CategoryUpadateViewResult Edit(CategoryMaster category);
        Task<CategoryMaster> Edit(CategoryMaster existingCategory, CategoryUpadateViewResult model);
        Task<List<CategoryMaster>> Index();
    }
}
