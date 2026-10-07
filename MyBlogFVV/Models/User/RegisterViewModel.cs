using System.ComponentModel.DataAnnotations;

namespace MyBlogFVV.WEB.Models.User
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Вам необходимо ввести имя")]
        [Display(Name = "Имя")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Длина должна быть от 1 до 50 символов")]
        public string FirstName { get; set; } = string.Empty;

        //[Required]
        [Display(Name = "Фамилия")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Длина должна быть от 1 до 50 символов")]
        public string? LastName { get; set; }

        //[Required]
        [Display(Name = "Отчество")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Длина должна быть от 1 до 50 символов")]
        public string? MiddleName { get; set; }

        [Required(ErrorMessage = "Вам необходимо ввести Email")]
        [EmailAddress(ErrorMessage = "Некорректный адрес")]
        [Display(Name = "Email")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Длина должна быть от 1 до 50 символов")]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "Дата рождения")]
        //[Required(ErrorMessage = "Вам необходимо ввести дату рождения")]
        [DataType(DataType.Date)] //Указываем, что нам нужна только дата
        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "Вам необходимо ввести пароль")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Длина должна быть от 1 до 50 символов")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Вам необходимо подтвердить пароль")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтвердить пароль")]
        public string PasswordConfirm { get; set; } = string.Empty;

        [Required(ErrorMessage = "Вам необходимо ввести логин")]
        [Display(Name = "Логин")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Длина должна быть от 1 до 50 символов")]
        public string Login { get; set; } = string.Empty;
    }
}
