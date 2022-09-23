using System.ComponentModel.DataAnnotations;

namespace Warehouse.Data.Dto
{
    public class TransactionCreateDto
    {
        public int? senderId { get; set; } = 0;
        public int receiverId { get; set; }
        public int productId { get; set; }

        public int Count { get; set; }
    }
}
