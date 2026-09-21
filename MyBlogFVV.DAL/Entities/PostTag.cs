
namespace MyBlogFVV.DAL.Entities
{
    public class PostTag
    {
        public int PostTagId { get; set; }
        public int PostId { get; set; }
        public Post Post { get; set; } = new Post();
        public int TagId { get; set; }
        public Tag Tag { get; set; } = new Tag();

    }
}
