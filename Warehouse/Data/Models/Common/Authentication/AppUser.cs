using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Warehouse.Data.Models.Common.Authentication
{
    public class AppUser: IdentityUser
    {
        public string Address { get; set; }
    }
}
