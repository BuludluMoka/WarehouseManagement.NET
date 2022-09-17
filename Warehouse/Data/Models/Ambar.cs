using Warehouse.Data.Models.Common;

namespace Warehouse.Data.Models
{
    public class Ambar:BaseEntity
    {
        public string Name { get; set; }
        public string Place { get; set; }

        public string Type { get; set; }
        public List<Transaction> Sender { get; set; }
        public List<Transaction> Receiver { get; set; }
        //public virtual ICollection<Transacction> receiverWarehouse { get; set; }
    }
}
