using Microsoft.EntityFrameworkCore;

namespace Warehouse.Data.Models.Common
{
    public class BaseEntity 
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }



    }
}
