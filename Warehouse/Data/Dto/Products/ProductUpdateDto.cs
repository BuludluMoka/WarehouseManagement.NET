using System.ComponentModel.DataAnnotations;

namespace Warehouse.Data.Dto.Products
{
    public class ProductUpdateDto
    {

        public string Name { get; set; }
        public float sellPrice { get; set; }
        public string? Description { get; set; }
        public int CategoryId { get; set; }

    }
}
