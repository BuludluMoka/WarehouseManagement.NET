using System.ComponentModel.DataAnnotations;

namespace Warehouse.Data.Dto.Products
{
    public class ProductUpdateDto
    {

        public string Name { get; set; }
        public float buyPrice { get; set; }
        public float sellPrice { get; set; }

    }
}
