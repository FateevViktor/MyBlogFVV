using MyBlogFVV.WEB.Models.Post;
using MyBlogFVV.WEB.Models.User;
using System.ComponentModel.DataAnnotations;

namespace MyBlogFVV.WEB.Models.Comment
{
    public class CommentViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Дата и время комментария")]
        [Required(ErrorMessage = "Вам необходимо ввести дату и время комментария")]
        [DataType(DataType.DateTime)]
        public DateTime CommentDate { get; set; }
        public UserViewModel Author { get; set; } = new UserViewModel();
        public PostViewModel Post { get; set; } = new PostViewModel();

        [Required(ErrorMessage = "Вам необходимо написать комментарий")]
        [Display(Name = "Комметарий")]
        [StringLength(200, MinimumLength = 1, ErrorMessage = "Длина комментария должна быть от 1 до 200 символов")]
        public string Text { get; set; } = string.Empty;
    }
}
