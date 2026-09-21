using System.ComponentModel.DataAnnotations;

namespace MyBlogFVV.WEB.Models.User
{
    public class UserEditViewModel
    {
        [Required]
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Имя")]
        public string FirstName { get; set; } = string.Empty;

        [Display(Name = "Фамилия")]
        public string? LastName { get; set; }

        [Display(Name = "Отчество")]
        public string? MiddleName { get; set; }

        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "Дата рождения")]
        [DataType(DataType.Date)] //Указываем, что нам нужна только дата
        public DateTime? BirthDate { get; set; }
    }
}
