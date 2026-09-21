
namespace MyBlogFVV.DAL.Entities
{
    public class Comment
    {
        public int CommentId { get; set; }
        public int PostId { get; set; }
        public Post Post { get; set; } = new Post();
        public int UserId { get; set; }
        public User User { get; set; } = new User();
        public string? Date { get; set; }
        public string? Text { get; set; }        
    }
}
