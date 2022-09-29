using Warehouse.Data.Models.Common;
using Warehouse.Data.Models.Common.Authentication;

namespace Warehouse.Data.Models
{
    public class Anbar:BaseEntity
    {
        public string Name { get; set; }
        public string Place { get; set; }
        public string  Phone { get; set; }
        public List<Transaction> Sender { get; set; }
        public List<Transaction> Receiver { get; set; }
        public List<AppUser> Users{ get; set; }
        //public virtual ICollection<Transacction> receiverWarehouse { get; set; }
    }
}
