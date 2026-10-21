
using System.ComponentModel.DataAnnotations;

namespace MyBlogFVV.API.Models.Tag
{
    public class TagEditModel
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "Вам необходимо придумать название тега")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Длина должна быть от 1 до 50 символов")]
        public string Text { get; set; } = string.Empty;
    }
}
