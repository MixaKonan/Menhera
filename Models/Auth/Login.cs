using System.ComponentModel.DataAnnotations;

namespace Menhera.Models.Auth
{
    public class Login
    {
        [Required(ErrorMessage = "Не указан Email.")]
        public string Email { get; set; }
         
        [Required(ErrorMessage = "Не указан пароль.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}