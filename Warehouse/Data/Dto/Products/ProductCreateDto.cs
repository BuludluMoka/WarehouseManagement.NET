using System.ComponentModel.DataAnnotations;

namespace Warehouse.Data.Dto.Products
{
    public class ProductCreateDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public float buyPrice { get; set; }
        public float sellPrice { get; set; }

        [Required]
        public int CategoryId { get; set; }
    }
}
