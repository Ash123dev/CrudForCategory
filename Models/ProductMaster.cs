using CrudForCategory.Request;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CrudForCategory.Models
{
    public class ProductMaster
    {
        [Key]
        public int Id { get; set; }
        public string ProductName { get; set; }
        public int CategoryId { get; set; }
        [ForeignKey(nameof(CategoryId))]
        public CategoryMaster Category { get; set; }
    }
}
