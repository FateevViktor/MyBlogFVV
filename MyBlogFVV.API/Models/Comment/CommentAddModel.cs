
using System.ComponentModel.DataAnnotations;

namespace MyBlogFVV.API.Models.Comment
{
    public class CommentAddModel
    {
        [Required(ErrorMessage = "Вам необходимо ввести комментарий")]
        [StringLength(250, MinimumLength = 1, ErrorMessage = "Длина должна быть от 1 до 250 символов")]
        public string CommentText { get; set; } = string.Empty;

        [Required(ErrorMessage = "Вам необходимо ввести автора комментария")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Длина должна быть от 1 до 50 символов")]
        public string AuthorLogin { get; set; } = string.Empty;

        [Range(1, int.MaxValue, ErrorMessage = "Значение должно быть не меньше 1")]
        public int PostId { get; set; }
    }
}
