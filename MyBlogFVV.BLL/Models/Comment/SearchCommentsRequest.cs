
namespace MyBlogFVV.BLL.Models.Comment
{
    public class SearchCommentsRequest
    {
        public List<CommentRequest> CommentList { get; set; } = new List<CommentRequest>();
    }
}
