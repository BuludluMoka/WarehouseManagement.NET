using Microsoft.AspNetCore.Identity;

namespace Warehouse.Data.Models.Common.Authentication
{
    public enum Roles
    {
        Admin,
        User
    }

    public class AppRole : IdentityRole
    {
        public AppRole()
        {

        }
        public AppRole(string RoleName) :base(RoleName)
        {

        }
    }
}
