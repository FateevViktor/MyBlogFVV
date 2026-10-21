using MyBlogFVV.API.Models.Tag;
using System.ComponentModel.DataAnnotations;

namespace MyBlogFVV.API.Models.Post
{
    public class PostAddApiModel
    {
        [Required(ErrorMessage = "Вам необходимо ввести автора статьи")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Длина должна быть от 1 до 50 символов")]
        public string AuthorLogin { get; set; } = string.Empty;

        [Required(ErrorMessage = "Вам необходимо придумать название статьи")]
        [StringLength(300, MinimumLength = 1, ErrorMessage = "Длина должна быть от 1 до 300 символов")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Вам необходимо написать статью")]
        [StringLength(100000, MinimumLength = 1, ErrorMessage = "Длина должна быть от 1 до 100000 символов")]
        public string Text { get; set; } = string.Empty;

        [Required(ErrorMessage = "Вам необходимо создать описание статьи")]
        [StringLength(1000, MinimumLength = 1, ErrorMessage = "Длина должна быть от 1 до 1000 символов")]
        public string Summary { get; set; } = string.Empty;
        public List<TagCheckedModel> Tag { get; set; } = [];
    }
}
