using System.ComponentModel.DataAnnotations;

namespace MyBlogFVV.WEB.Models.Tag
{
    public class TagEditViewModel
    {
        [Required]
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Вам необходимо придумать название тега")]
        [Display(Name = "Название тега")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Длина должна быть от 1 до 50 символов")]
        public string Text { get; set; } = string.Empty;
    }
}
