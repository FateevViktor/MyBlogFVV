using MyBlogFVV.BLL.Models.Comment;
using MyBlogFVV.BLL.Models.User;

namespace MyBlogFVV.BLL.Models.Post
{
    public class PostRequest
    {
        public int Id { get; set; }
        public DateTime? PostDate { get; set; }
        public UserRequest Author { get; set; } = new UserRequest();
        public string? Title { get; set; }
        public string? Text { get; set; }
        public List<CommentRequest> Comment { get; set; } = new List<CommentRequest>();
        //public List<TagRequest> Tag { get; set; } = new List<TagRequest>();
    }
}
