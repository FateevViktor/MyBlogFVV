using System.ComponentModel.DataAnnotations;

namespace MyBlogFVV.WEB.Models.Role
{
    public class RoleViewModel
    {
        [Required]
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Вам необходимо придумать название роли")]
        [Display(Name = "Название роли")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Длина должна быть от 1 до 50 символов")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Вам необходимо придумать описание роли")]
        [Display(Name = "Описание роли")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Длина должна быть от 1 до 50 символов")]
        public string Description { get; set; } = string.Empty;
    }
}
