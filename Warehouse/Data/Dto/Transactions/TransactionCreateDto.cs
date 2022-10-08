using System.ComponentModel.DataAnnotations;

namespace Warehouse.Data.Dto
{
    public class TransactionCreateDto
    {
        
        public bool Sender { get; set; } 
        public int Receiver { get; set; }
        public int productId { get; set; }
        

        public int Count { get; set; }
    }
}
