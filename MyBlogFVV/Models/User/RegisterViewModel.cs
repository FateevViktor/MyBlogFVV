using System.ComponentModel.DataAnnotations;

namespace MyBlogFVV.WEB.Models.User
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Вам необходимо ввести имя")]
        [Display(Name = "Имя")]
        public string FirstName { get; set; } = string.Empty;

        //[Required]
        [Display(Name = "Фамилия")]
        public string? LastName { get; set; }

        //[Required]
        [Display(Name = "Отчество")]
        public string? MiddleName { get; set; }

        [Required(ErrorMessage = "Вам необходимо ввести Email")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "Дата рождения")]
        //[Required(ErrorMessage = "Вам необходимо ввести дату рождения")]
        [DataType(DataType.Date)] //Указываем, что нам нужна только дата
        public DateTime? BirthDate { get; set; }

        [Required(ErrorMessage = "Вам необходимо ввести пароль")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        [StringLength(100, ErrorMessage = "Поле {0} должно иметь минимум {2} и максимум {1} символов.", MinimumLength = 5)]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Вам необходимо подтвердить пароль")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтвердить пароль")]
        public string PasswordConfirm { get; set; } = string.Empty;

        [Required(ErrorMessage = "Вам необходимо ввести логин")]
        [Display(Name = "Логин")]
        public string Login { get; set; } = string.Empty;
    }
}
