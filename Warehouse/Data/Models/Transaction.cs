using System.ComponentModel.DataAnnotations.Schema;
using Warehouse.Data.Models.Common;
using Warehouse.Data.Models.Common.Authentication;

namespace Warehouse.Data.Models
{

    public class Transaction: BaseEntity
    {
        public string TransactionNo { get; set; }
        public int? sender_id { get; set; }
        public int receiver_id { get; set; }
        public string UserId { get; set; }
        public int ProductId { get; set; }
        public bool Status { get; set; } = false;
        public int Count { get; set; }
        public Product Product { get; set; }
        public Anbar Sender { get; set; }
        
        public Anbar Receiver { get; set; }
        public AppUser User { get; set; }




    }
}
