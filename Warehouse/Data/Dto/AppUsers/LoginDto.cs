using System.ComponentModel.DataAnnotations;

namespace Warehouse.Data.Dto.AppUsers
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Xais edirik Email adressini bos kecmeyin")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Xais edirik uygun formatda email adresi giriniz")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Xais edirik parolu bos kecmeyin")]
        [DataType(DataType.Password, ErrorMessage = "Xais edirik uygun formatda parol giriniz")]
        public string Password { get; set; }
    }
}
