using System.ComponentModel.DataAnnotations.Schema;
using Warehouse.Data.Models.Common;

namespace Warehouse.Data.Models
{

    public class Transaction: BaseEntity
    {
        public string TransactionNo { get; set; }
        //[ForeignKey("Sender")]


        //[ForeignKey("Receiver")]

        public int? sender_id { get; set; }
        public int receiver_id { get; set; }
        [ForeignKey("Product")]
        public int product_id { get; set; }
        public Product Product { get; set; }
        public bool Status { get; set; } = false;

        public int Count { get; set; }
        
        public Anbar Sender { get; set; }
        
        public Anbar Receiver { get; set; }




    }
}
