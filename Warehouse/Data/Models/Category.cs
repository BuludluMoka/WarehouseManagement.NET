using Warehouse.Data.Models.Common;

namespace Warehouse.Data.Models
{
    public sealed class Category:BaseEntity
    {
        public Category()
        {
            categoryChildren = new List<Category>();
        }
        public string Name { get; set; }
        public int? ParentId { get; set; }
        List<Product> Products { get; set; }
        public Category Parent { get; set; }
        public List<Category> categoryChildren{ get; set; }

    }
}
