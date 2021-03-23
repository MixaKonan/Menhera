using System.ComponentModel.DataAnnotations;

namespace Menhera.Models.Auth
{
    public class Request
    {
        [Required(ErrorMessage ="Не указан логин.")]
        [DataType(DataType.Text)]
        public string Login { get; set; }
        
        [Required(ErrorMessage ="Не указан Email.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
         
        [Required(ErrorMessage = "Не указан пароль.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
         
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароль введён неверно.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage ="Обязательно.")]
        [DataType(DataType.Text)]
        public string Comment { get; set; }
    }
}