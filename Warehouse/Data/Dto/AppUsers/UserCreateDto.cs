using System.ComponentModel.DataAnnotations;

namespace Warehouse.Data.Dto.AppUsers
{
    public class UserCreateDto
    {
        [Required(ErrorMessage = "Xais edirik isdifadeci adinizi bos kecmeyin...")]
        [StringLength(15, ErrorMessage = "Isdifadeci adinizi 4 ile 15 herf arasi giriniz...", MinimumLength = 4)]
        public string UserName { get; set; }
        /// <summary>
        /// /////////////////
        /// </summary>

        [Required(ErrorMessage = "Bos kecmeyin")]
        [EmailAddress(ErrorMessage = "Xais edirik email formatinda bir email giriniz...")]
        public string Email { get; set; }

        /// <summary>
        /// //////////
        /// </summary>
        [Required(ErrorMessage = "Xais edirik isdifadeci parolunu bos kecmeyin")]
        [DataType(DataType.Password, ErrorMessage = "Xais edirik parolu butun qaydalara uygul formada giriniz...")]
        public string PasswordHash { get; set; }


        public string PhoneNumber { get; set; }


        public string Address { get; set; }
    }
}

