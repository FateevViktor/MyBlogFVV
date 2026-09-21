
namespace MyBlogFVV.DAL.Entities
{
    public class Tag
    {
        public int TagId { get; set; }
        public string? Text { get; set; }
        public IEnumerable<PostTag> PostTags { get; set; } = new List<PostTag>();
    }
}
