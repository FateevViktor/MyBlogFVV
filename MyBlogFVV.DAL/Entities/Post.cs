
namespace MyBlogFVV.DAL.Entities
{
    public class Post
    {
        public int PostId { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
        public string? Title { get; set; }
        public string? Text { get; set; }
        public string? Date { get; set; }
        public IEnumerable<Comment> Comments { get; set; } = new List<Comment>();
        public IEnumerable<PostTag> PostTags { get; set; } = new List<PostTag>();

    }
}
