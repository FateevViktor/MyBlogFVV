using MyBlogFVV.API.Models.Post;
using MyBlogFVV.API.Models.User;
using System.ComponentModel.DataAnnotations;

namespace MyBlogFVV.API.Models.Comment
{
    public class CommentModel
    {
        public int Id { get; set; }

        [Display(Name = "Дата и время комментария")]
        [Required(ErrorMessage = "Вам необходимо ввести дату и время комментария")]
        [DataType(DataType.DateTime)]
        public DateTime CommentDate { get; set; }
        public UserModel Author { get; set; } = new UserModel();
        public PostModel Post { get; set; } = new PostModel();

        [Required(ErrorMessage = "Вам необходимо написать комментарий")]
        [StringLength(200, MinimumLength = 1, ErrorMessage = "Длина комментария должна быть от 1 до 200 символов")]
        public string Text { get; set; } = string.Empty;
    }
}
