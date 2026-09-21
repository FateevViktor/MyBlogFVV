using System.ComponentModel.DataAnnotations;

namespace MyBlogFVV.WEB.Models.User
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Вам необходимо ввести пароль")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        [StringLength(100, ErrorMessage = "Поле {0} должно иметь минимум {2} и максимум {1} символов.", MinimumLength = 5)]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Вам необходимо ввести логин")]
        [Display(Name = "Логин")]
        public string Login { get; set; } = string.Empty;

        //[Required]
        [Display(Name = "Запомнить меня")]
        public bool RememberMe { get; set; } = false;

        //[Required]
        //[Display(Name = "Никнейм")]
        public string ReturnUrl { get; set; } = string.Empty;
    }
}
