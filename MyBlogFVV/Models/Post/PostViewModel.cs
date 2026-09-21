using MyBlogFVV.WEB.Models.Comment;
using MyBlogFVV.WEB.Models.User;

namespace MyBlogFVV.WEB.Models.Post
{
    public class PostViewModel
    {
        public int Id { get; set; }
        public DateTime? PostDate { get; set; }
        public UserViewModel Author { get; set; } = new UserViewModel();
        public string? Title { get; set; }
        public string? Text { get; set; } = string.Empty;
        public List<CommentViewModel> Comment { get; set; } = new List<CommentViewModel>();
        //public List<TagViewModel> Tag { get; set; } = new List<TagViewModel>();
    }
}
