using MyBlogFVV.WEB.Models.Comment;
using MyBlogFVV.WEB.Models.Tag;
using MyBlogFVV.WEB.Models.User;

namespace MyBlogFVV.WEB.Models.Post
{
    public class PostViewModel
    {
        public int Id { get; set; }
        public DateTime? PostDate { get; set; }
        public UserViewModel Author { get; set; } = new UserViewModel();
        public string Title { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public string Summary { get; set; } = string.Empty;
        public List<CommentViewModel> Comment { get; set; } = [];
        public List<TagViewModel> Tag { get; set; } = [];
    }
}
