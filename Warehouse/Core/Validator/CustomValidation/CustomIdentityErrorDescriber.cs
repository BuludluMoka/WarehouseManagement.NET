using Microsoft.AspNetCore.Identity;

namespace Warehouse.Core.Validator.CustomValidation
{
    public class CustomIdentityErrorDescriber: IdentityErrorDescriber
    {
        public override IdentityError DuplicateUserName(string userName) => new IdentityError { Code = "DuplicateUserName", Description = $"'{userName}' Bu isdifadeci adi hal hazirda isdifade olunur." };
        public override IdentityError InvalidUserName(string userName) => new IdentityError { Code = "InvalidUserName", Description = "Yanlis isdifadeci adi." };
        public override IdentityError DuplicateEmail(string email) => new IdentityError { Code = "DuplicateEmail", Description = $"'{email}' başka bir isdifadeci treefinden isdifade olunur." };
        public override IdentityError InvalidEmail(string email) => new IdentityError { Code = "InvalidEmail", Description = "Yanlis email." };
        public override IdentityError PasswordTooShort(int length) => new IdentityError { Code = "ShortPassword", Description = "Parol cox qisadir" };

    }
}
