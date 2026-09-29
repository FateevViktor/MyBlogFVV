using System.ComponentModel.DataAnnotations;

namespace MyBlogFVV.WEB.Models.User
{
    public class UserEditViewModel
    {
        [Required]
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Вам необходимо ввести имя")]
        [Display(Name = "Имя")]
        public string FirstName { get; set; } = string.Empty;

        [Display(Name = "Фамилия")]
        public string? LastName { get; set; }

        [Display(Name = "Отчество")]
        public string? MiddleName { get; set; }

        [Required(ErrorMessage = "Вам необходимо ввести Email")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "Дата рождения")]
        [DataType(DataType.Date)] //Указываем, что нам нужна только дата
        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "Вам необходимо ввести логин")]
        [Display(Name = "Login")]
        public string Login { get; set; } = string.Empty;

        public List<string> Roles { get; set; } = [];

    }
}
