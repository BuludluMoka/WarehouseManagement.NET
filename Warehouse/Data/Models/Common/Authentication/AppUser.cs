using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Warehouse.Data.Models.Common.Authentication
{
    public class AppUser: IdentityUser
    {
        

        [ForeignKey("Anbar")]
        public int AnbarId { get; set; }
        public string Address { get; set; }
        public bool Status { get; set; } = true;
        public string? ResetPassword { get; set; }
        public Anbar Anbar { get; set; }
        public List<Transaction> Transactions { get; set; }

    }
}
