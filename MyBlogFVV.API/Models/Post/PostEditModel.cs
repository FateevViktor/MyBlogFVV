using MyBlogFVV.API.Models.Comment;
using MyBlogFVV.API.Models.Tag;
using MyBlogFVV.API.Models.User;
using System.ComponentModel.DataAnnotations;

namespace MyBlogFVV.API.Models.Post
{
    public class PostEditModel
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "Необходимо ввести дату поста")]
        [DataType(DataType.Date)]
        public DateTime? PostDate { get; set; }
        public UserModel Author { get; set; } = new UserModel();

        [Required(ErrorMessage = "Вам необходимо придумать название статьи")]
        [StringLength(300, MinimumLength = 1, ErrorMessage = "Длина должна быть от 1 до 300 символов")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Вам необходимо написать статью")]
        [StringLength(100000, MinimumLength = 1, ErrorMessage = "Длина должна быть от 1 до 100000 символов")]
        public string Text { get; set; } = string.Empty;

        [Required(ErrorMessage = "Вам необходимо создать описание статьи")]
        [StringLength(1000, MinimumLength = 1, ErrorMessage = "Длина должна быть от 1 до 1000 символов")]
        public string Summary { get; set; } = string.Empty;
        public List<CommentModel> Comment { get; set; } = [];
        public List<TagCheckedModel> Tag { get; set; } = [];
    }
}
