using MyBlogFVV.WEB.Models.Comment;
using MyBlogFVV.WEB.Models.Tag;
using MyBlogFVV.WEB.Models.User;
using System.ComponentModel.DataAnnotations;

namespace MyBlogFVV.WEB.Models.Post
{
    public class PostEditViewModel
    {
        [Required]
        [Display(Name = "Id")]
        public int Id { get; set; }

        [DataType(DataType.Date)] //Указываем, что нам нужна дата
        public DateTime? PostDate { get; set; }
        public UserViewModel Author { get; set; } = new UserViewModel();
        
        [Required(ErrorMessage = "Вам необходимо придумать название статьи")]
        [Display(Name = "Название статьи")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Вам необходимо написать статью")]
        [Display(Name = "Текст статьи")]
        public string Text { get; set; } = string.Empty;

        [Required(ErrorMessage = "Вам необходимо создать описание статьи")]
        [Display(Name = "Краткое описание статьи")]
        public string Summary { get; set; } = string.Empty;
        public List<CommentViewModel> Comment { get; set; } = [];
        public List<TagCheckedViewModel> Tag { get; set; } = [];
    }
}
