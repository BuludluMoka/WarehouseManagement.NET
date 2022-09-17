using Warehouse.Data.Models.Common;

namespace Warehouse.Data.Models
{
    public class Category:BaseEntity
    {
        public string Name { get; set; }
        public int parent_id { get; set; }
        ICollection<Product> Products { get; set; }


    }
}
