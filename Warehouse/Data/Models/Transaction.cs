using System.ComponentModel.DataAnnotations.Schema;
using Warehouse.Data.Models.Common;

namespace Warehouse.Data.Models
{

    public class Transaction: BaseEntity
    {
        //[ForeignKey("Sender")]
        public int? sender_id { get; set; }
        public Ambar Sender { get; set; }
        
        //[ForeignKey("Receiver")]
        public int receiver_id { get; set; }
        public Ambar Receiver { get; set; }

        [ForeignKey("Product")]
        public int product_id { get; set; }
        public Product Product { get; set; }

        public int Count { get; set; }
        


    }
}
