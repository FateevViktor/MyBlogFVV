using MyBlogFVV.BLL.Models.Post;
using MyBlogFVV.BLL.Models.User;

namespace MyBlogFVV.BLL.Models.Comment
{
    public class CommentRequest
    {
        public int Id { get; set; }
        public DateTime CommentDate { get; set; }
        public UserRequest Author { get; set; }
        public PostRequest Post { get; set; }
        public string Text { get; set; } = string.Empty;
    }
}
