namespace Warehouse.Data.Dto.AppUsers
{
    public class ChangeUserPasswordDto
    {
        public string Email { get; set; }
        public string newPassword { get; set; }
        public string PasswordConfirmation { get; set; }
    }
}
