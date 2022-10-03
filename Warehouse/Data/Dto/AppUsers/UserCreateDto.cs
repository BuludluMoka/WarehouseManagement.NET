using System.ComponentModel.DataAnnotations;

namespace Warehouse.Data.Dto.AppUsers
{
    public class UserCreateDto
    {
        [Required(ErrorMessage = "Xais edirik isdifadeci adinizi bos kecmeyin...")]
        [StringLength(15, ErrorMessage = "Isdifadeci adinizi 4 ile 15 herf arasi girin...", MinimumLength = 4)]
        public string UserName { get; set; }
      

        [Required(ErrorMessage = "Bos kecmeyin")]
        [EmailAddress(ErrorMessage = "Xais edirik email formatinda bir email girin...")]
        public string Email { get; set; }

     
        [Required(ErrorMessage = "Xais edirik isdifadeci parolunu bos kecmeyin")]
        [DataType(DataType.Password, ErrorMessage = "Xais edirik parolu butun qaydalara uygul formada girin...")]
        public string PasswordHash { get; set; }

        [Required(ErrorMessage = "Xais edirik Userin Anbarini secin...")]
        public int AnbarId { get; set; }


        public string PhoneNumber { get; set; }


        public string Address { get; set; }
    }
}

