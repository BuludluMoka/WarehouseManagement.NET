using System.ComponentModel.DataAnnotations;

namespace Warehouse.Data.Dto.Products
{
    public class ProductCreateDto
    {
        [Required]
        public string Name { get; set; }
        public float buyPrice { get; set; }
        public float sellPrice { get; set; }

        public int category_Id { get; set; }
    }
}
