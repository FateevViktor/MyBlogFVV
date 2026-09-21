using MyBlogFVV.WEB.Models.Post;
using MyBlogFVV.WEB.Models.User;
using System.ComponentModel.DataAnnotations;

namespace MyBlogFVV.WEB.Models.Comment
{
    public class CommentViewModel
    {
        public int Id { get; set; }
        public DateTime CommentDate { get; set; }
        public UserViewModel Author { get; set; } = new UserViewModel();
        public PostViewModel Post { get; set; } = new PostViewModel();

        [Required]
        public string Text { get; set; } = string.Empty;
    }
}
