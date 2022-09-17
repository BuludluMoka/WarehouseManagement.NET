using System.ComponentModel.DataAnnotations;

namespace Warehouse.Data.Dto
{
    public class TransactionCreateDto
    {
        public int? senderId { get; set; } = 0;
        [Required]
        public int receiverId { get; set; }
        [Required]
        public int productId { get; set; }

        [Required]
        public int Count { get; set; }
    }
}
