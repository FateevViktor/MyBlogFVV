using MyBlogFVV.API.Models.Comment;
using MyBlogFVV.API.Models.Tag;
using MyBlogFVV.API.Models.User;

namespace MyBlogFVV.API.Models.Post
{
    public class PostAddModel
    {
        public int Id { get; set; }
        public DateTime? PostDate { get; set; }
        public UserModel Author { get; set; } = new UserModel();
        public string Title { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public string Summary { get; set; } = string.Empty;
        public List<CommentModel> Comment { get; set; } = [];
        public List<TagCheckedModel> Tag { get; set; } = [];
    }
}
