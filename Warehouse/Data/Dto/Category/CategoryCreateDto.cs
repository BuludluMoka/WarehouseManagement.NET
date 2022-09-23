using System.ComponentModel.DataAnnotations;

namespace Warehouse.Data.Dto.Category
{
    public class CategoryCreateDto
    {
        [Required(ErrorMessage = "Kateqorya adini bos kecmeyin")]
        public string Name { get; set; }
        public int? parentId { get; set; }
    }
}
