namespace Warehouse.Data.Dto.Transactions
{
    public class TransactionUpdateDto
    {
        public int senderId { get; set; }
        public int receiverId { get; set; }
        public int productId { get; set; }

        public int Count { get; set; }
    }
}
