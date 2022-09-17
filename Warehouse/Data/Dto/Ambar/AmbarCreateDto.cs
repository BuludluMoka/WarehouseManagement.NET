using System.ComponentModel.DataAnnotations;

namespace Warehouse.Data.Dto
{
    public class AmbarCreateDto
    {
        [Required]
        public string Name { get; set; }
        public string Place { get; set; }
        public string Type { get; set; }

    }
}
