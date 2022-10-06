namespace Warehouse.Data.Dto.AppUsers
{
    public class UserUpdateDto
    {
        public string Email { get; set; }
        public string OldPassword { get; set; }
        public string  NewPassword { get; set; }

    }
}
