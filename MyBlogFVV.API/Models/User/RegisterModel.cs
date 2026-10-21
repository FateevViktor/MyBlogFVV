
using System.ComponentModel.DataAnnotations;

namespace MyBlogFVV.API.Models.User
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Вам необходимо ввести имя")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Длина должна быть от 1 до 50 символов")]
        public string FirstName { get; set; } = string.Empty;

        [StringLength(50, MinimumLength = 1, ErrorMessage = "Длина должна быть от 1 до 50 символов")]
        public string? LastName { get; set; }

        [StringLength(50, MinimumLength = 1, ErrorMessage = "Длина должна быть от 1 до 50 символов")]
        public string? MiddleName { get; set; }

        [Required(ErrorMessage = "Вам необходимо ввести Email")]
        [EmailAddress(ErrorMessage = "Некорректный адрес")]
        public string Email { get; set; } = string.Empty;

        [DataType(DataType.Date)] //Указываем, что нам нужна только дата
        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "Вам необходимо ввести пароль")]
        [DataType(DataType.Password)]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Длина должна быть от 5 до 50 символов")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Вам необходимо подтвердить пароль")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        public string PasswordConfirm { get; set; } = string.Empty;

        [Required(ErrorMessage = "Вам необходимо ввести логин")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Длина должна быть от 1 до 50 символов")]
        public string Login { get; set; } = string.Empty;
    }
}
