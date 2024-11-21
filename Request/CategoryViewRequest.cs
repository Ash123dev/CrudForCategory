using CrudForCategory.Models;
using System.ComponentModel.DataAnnotations;

namespace CrudForCategory.Request
{
	public class CategoryViewRequest
	{
		[Required(ErrorMessage = "Category Name is required")]
		public string CategoryName { get; set; }
		[MinLength(1, ErrorMessage = "At least one product name is required")]
		public List<string> ProductNames { get; set; }
	}
}
