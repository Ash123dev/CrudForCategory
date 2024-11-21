using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;

namespace CrudForCategory.Models
{
    public class CategoryMaster
    {
        [Key]
        public int Id { get; set; } 
        public string CategoryName { get; set; }
        public List<ProductMaster> ProductMasters { get; set; }
    }
}
