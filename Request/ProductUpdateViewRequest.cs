namespace CrudForCategory.Request
{
	public class ProductUpdateViewRequest
	{
		public int? Id { get; set; } 
		public string ProductName { get; set; }
		public int CategoryId { get; set; }
	}
}
