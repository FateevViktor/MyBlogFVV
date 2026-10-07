using MyBlogFVV.WEB.Models.Comment;
using MyBlogFVV.WEB.Models.Tag;
using MyBlogFVV.WEB.Models.User;
using System.ComponentModel.DataAnnotations;

namespace MyBlogFVV.WEB.Models.Post
{
    public class PostAddViewModel
    {
        [Required]
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Необходимо ввести дату поста")]
        [DataType(DataType.Date)]
        public DateTime? PostDate { get; set; }

        public UserViewModel Author { get; set; } = new UserViewModel();

        [Required(ErrorMessage = "Вам необходимо придумать название статьи")]
        [Display(Name = "Название статьи")]
        [StringLength(300, MinimumLength = 1, ErrorMessage = "Длина должна быть от 1 до 300 символов")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Вам необходимо написать статью")]
        [Display(Name = "Текст статьи")]
        [StringLength(100000, MinimumLength = 1, ErrorMessage = "Длина должна быть от 1 до 100000 символов")]
        public string Text { get; set; } = string.Empty;

        [Required(ErrorMessage = "Вам необходимо создать описание статьи")]
        [Display(Name = "Краткое описание статьи")]
        [StringLength(1000, MinimumLength = 1, ErrorMessage = "Длина должна быть от 1 до 1000 символов")]
        public string Summary { get; set; } = string.Empty;
        public List<CommentViewModel> Comment { get; set; } = [];
        public List<TagCheckedViewModel> Tag { get; set; } = [];
    }
}
